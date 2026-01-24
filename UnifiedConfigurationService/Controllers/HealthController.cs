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
[Route( "health" )]
public class HealthController : Controller
{
    [AllowAnonymous]
    [HttpGet( "Ping" )]
    public string Ping()
    {        
        return "Pong - YoyoDyne - The future begins tomorrow";
    }

    [AllowAnonymous]
    [HttpGet]
    public IResult HealthCheck()
    {        
        try
        { 
            var dbConfig = ConfigDatabase.Instance;
            dbConfig.ConnectionGet();

            return Results.Ok(new { status = "Healthy" } );
        }
        catch (Exception ex)
        {
            return Results.Json( new { status = "Unhealthy" }, statusCode: 503 );
        }
        
    }
}

