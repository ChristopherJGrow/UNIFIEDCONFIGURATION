namespace UnifiedConfigurationService.Controllers;

using Config.Core;
using Config.Core.Auth;
using Config.Core.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Models;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

using UnifiedConfiguration.Business;


[ApiController]
[Route( "auth" )]
public class AuthController : ControllerBase
{
    [Authorize]
    [HttpPost( "IsDefaultAuthorized" )]
    public bool IsDefaultAuthorized()
    {
        var retval = User.FindFirstValue( UcsClaimTypes.IsDefaultAuth )?.ToBool() ?? false;

        return retval;
    }



    [AllowAnonymous]
    [HttpGet( "token" )]
    public IActionResult GetDevToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_secret_dev_key_12345_67890_ABCDE"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var sid = WindowsIdentity.GetCurrent().User.Value;
        var claims = new[]
    {
        //new Claim("sid", "S-1-5-21-000000001"),

        //new Claim("env", "DEV"),
        new Claim( UcsClaimTypes.Enviornment, "DEV" ),

        //new Claim("app", "IMS"),
        new Claim( UcsClaimTypes.Application, "IMS" ),

        //new Claim("mod", "PrintAgent"),
        new Claim( UcsClaimTypes.Module, "PrintAgent"),

        //new Claim("ver", "9.3.0.0"),
        //new Claim( UcsClaimTypes.BuildNumber, "1200"),

        //new Claim("defaultAuth", "true")
        new Claim(UcsClaimTypes.IsDefaultAuth,  "true"),
    };

        var token = new JwtSecurityToken(
        issuer: "dev-issuer",
        audience: "dev-audience",
        claims: claims,
        expires: DateTime.UtcNow.AddHours(8),
        signingCredentials: creds);

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok( jwt );
    }


    /// <summary>
    /// Returns a JWT based on request data and windows AUTH
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost( "TokenCreate" )]
    public async Task<TokenResponse> TokenCreate([FromBody] TokenRequest request)
    {
        string accessToken=string.Empty;

        //var name = WindowsIdentity.GetCurrent().Name;

        var userId = request.UserId ?? ""; // WindowsIdentity.GetCurrent().User.Value;

        //WindowsPrincipal wp = new WindowsPrincipal( WindowsIdentity.GetCurrent() );
        //var globalDefaults = CapabilityChecker.UserHasCapability( wp,  "Config.EditGlobalDefaults" );


        var isAdmin = ConfigDatabase.Instance.ExecuteScalar<bool>( "dbo.UCS_IsAdmin", userId);

        var tConfig= new CTokenConfigJWT()
        {
            Claims = new Claim[]
                {
                    new Claim(UcsClaimTypes.Enviornment,    $"{request.Environment}"),
                    new Claim(UcsClaimTypes.Application,    $"{request.Application}"),
                    new Claim(UcsClaimTypes.Module,         $"{request.Module}"),                        
                    new Claim(UcsClaimTypes.UserId,         $"{userId}"),
                    new Claim(UcsClaimTypes.IsDefaultAuth,  $"{isAdmin}")
                }
        };

        var tokenManager = CTokenMgr.Instance;

        accessToken = tokenManager.TokenGenerate( tConfig );

        return new TokenResponse( accessToken ) { IsAdmin = isAdmin };
    }

    [Authorize]
    [HttpGet( "TokenDetails" )]
    public ActionResult <TokenDetailsResult> TokenDetails()
    {
        var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
        var app = User.FindFirstValue( UcsClaimTypes.Application );
        var mod = User.FindFirstValue( UcsClaimTypes.Module );
        //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
        var uid = User.FindFirstValue( UcsClaimTypes.UserId );

        var retval = new TokenDetailsResult()
        {
            Environment = env,
            Application = app,
            Module = mod,
            //BuildNumber = ver,
            UserId = uid,
        };

        return Ok( retval );
    }

      

}





