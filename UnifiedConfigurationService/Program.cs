

using Config.Core.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

Console.Title = "UnifiedConfiguration";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.WebHost.UseUrls( "http://0.0.0.0:5191" );

builder.Services.AddControllers().AddJsonOptions( O=>
{
    O.JsonSerializerOptions.PropertyNamingPolicy=null;
    O.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
} );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen( c =>
{
    c.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement( new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    } );
} );

builder.Services
    .AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
      .AddJwtBearer( JwtBearerDefaults.AuthenticationScheme, cfg =>
        {
            var tSvc = CTokenMgr.Instance;
            cfg.TokenValidationParameters = tSvc.TokenValidationParametersGet();

            // TEMP: add logging so we can see why it fails
            cfg.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    var auth = ctx.Request.Headers["Authorization"].ToString();
                    Console.WriteLine( "Authorization header: " + (string.IsNullOrEmpty( auth ) ? "<none>" : auth) );
                    Console.WriteLine( "ctx.Token in OnMessageReceived: " + (ctx.Token ?? "<null>") );
                    return Task.CompletedTask;
                },

                OnTokenValidated = ctx =>
                {
                    // This runs *after* the token has been read and validated
                    var name = ctx.Principal?.Identity?.Name ?? "<no name>";
                    Console.WriteLine( $"JWT token validated for: {name}" );
                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = ctx =>
                {
                    Console.WriteLine( "JWT auth failed: " + ctx.Exception );
                    return Task.CompletedTask;
                },

                OnChallenge = ctx =>
                {
                    Console.WriteLine( "JWT challenge: " + ctx.Error + " - " + ctx.ErrorDescription );
                    return Task.CompletedTask;
                }
            };

        } );

//builder.Services.AddAuthorization();
builder.Services.AddAuthorization( options =>
{
    options.AddPolicy( "RequiresApp", policy =>
    {
        policy.RequireClaim( UcsClaimTypes.Application );
    } );
    options.AddPolicy( "RequiresSid", policy =>
    {
        policy.RequireClaim( UcsClaimTypes.UserId );
    } );
    options.AddPolicy( "RequiresDefaultAuth", policy =>
    {
        policy.RequireClaim( UcsClaimTypes.IsDefaultAuth );
    } );

    // Maybe??
    options.AddPolicy( "CanEditDefaults", policy => policy.RequireAssertion( ctx =>
    {
        var user = ctx.User.Identity?.Name; // or claim like "sub"
        // Call a service that checks DB table of allowed editors
        return user != null && ctx.User.HasClaim( "can_edit_defaults", "true" ); // or inject service
    } ) );


} );

var app = builder.Build();

// Configure the HTTP request pipeline.
// Put this back in..
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
//app.UseHttpsRedirection();

app.UseAuthentication();   // IMPORTANT: must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
