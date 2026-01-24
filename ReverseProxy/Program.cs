using Core;


using Microsoft.AspNetCore.HttpOverrides;


// See https://aka.ms/new-console-template for more information

Console.Title = "Reverse Proxy";

Console.WriteLine( "Reverse Proxy Starting" );

var builder = WebApplication.CreateBuilder(args);

// Load YARP routes/clusters from configuration (appsettings.json)

builder.Services.AddReverseProxy().LoadFromConfig( builder.Configuration.GetSection( "ReverseProxy" ) );

// Optional but recommended (helps apps downstream know origional scheme/client IP later)
builder.Services.Configure<ForwardedHeadersOptions>( O =>
{
    O.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


var app = builder.Build();

app.UseForwardedHeaders();

// this is the actual proxy pipeline
app.MapReverseProxy();

app.Run();