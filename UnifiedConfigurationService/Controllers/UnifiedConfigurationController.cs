

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

using Models;

using Config.Core;
using Config.Core.Extensions;
using Config.Core.Auth;
using UnifiedConfiguration.Business;

namespace UnifiedConfigurationService.Controllers
{



    
    [ApiController]    
    [Route("config")]
    //[Authorize( Policy = "RequiresApp" )]
    public class UnifiedConfigurationController : ControllerBase
    {
        //[Authorize]
        [HttpGet( "GetUniqueSections" )]
        public ActionResult<UniqueSectionsResult> GetUniqueSections()
        {
            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
            var mod = User.FindFirstValue( UcsClaimTypes.Module );            
            var uid = User.FindFirstValue( UcsClaimTypes.UserId );

            return ConfigResolver.Instance.GetUniqueSections( env, app );


            //var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetUniqueSections",env, app );
            //DataTable table = dataSet.Tables[0];
            //var result = table.AsEnumerable()
            //                    .Select(row => row.GetField("Section", "") )
            //                    .ToList();

            //return Ok(new UniqueSectionsResult() { Sections = result } );
        }

        /// <summary>
        /// Returns a list of variable and value pairs for a given section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet( "GetSectionSettings" )]
        public IResult GetSectionSettings(string section, string buildNumber)
        {
            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );            
            var uid = User.FindFirstValue( UcsClaimTypes.UserId );
            var ver = buildNumber; // ?? User.FindFirstValue( UcsClaimTypes.BuildNumber );

            return Results.Ok(ConfigResolver.Instance.GetSectionSettings(env,app,mod,section,ver,uid) );

            //var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetSectionSettings",env, section, app, ver.ToInt(), mod, uid );

            //DataTable table = dataSet.Tables[0];
            //var result = table
            //        .AsEnumerable()
            //        .Select(row =>                    
            //        new SettingGetResult()
            //        {
            //            Variable = row.GetField("Variable","" ),
            //            Value = row.GetField("Value", "")   ,
            //            OverridingUserId = row.GetField("OverridingUserId",""),
            //            EffectiveBuildNumber =  row.GetField("EffectiveBuildNumber","" ),
            //            EffectiveModule = row.GetField("EffectiveModule","" ),
                        
            //        } );
                    
            //return new SectionResult()
            //            {
            //                Environment = env,
            //                Application = app,
            //                Module = mod,
            //                Settings = result.ToList()
            //            };
        }

        [Authorize]
        [HttpGet( "GetSectionSettingsAllBuilds" )]
        public IResult GetSectionSettingsAllBuilds(string section)
        {
            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );            
            var uid = User.FindFirstValue( UcsClaimTypes.UserId );

            return Results.Ok( ConfigResolver.Instance.GetSectionSettingsAllBuilds( env, app, mod, section ) );

            //var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetSectionSettingsAllBuilds",env, section, app,  mod  );
            //DataTable table = dataSet.Tables[0];
            //var result = table
            //        .AsEnumerable()
            //        .Select(row =>
            //        new SettingGetResult()
            //        {
            //            Variable = row.GetField("Variable","" ),
            //            Value = row.GetField("Value", "")   ,
            //            //OverridingUserId = row.GetField("OverridingUserId",""),
            //            EffectiveBuildNumber =  row.GetField("BuildNumber","" ),
            //            EffectiveModule = row.GetField("ModuleName","" ),                        
            //            OverridingUserId = row.GetField("UserId","")
            //        } );

            //return Results.Ok(new SectionResult()
            //{
            //    Environment = env,
            //    Application = app,
            //    Module = mod,
            //    Settings = result.ToList()
            //} );
        }

        /// <summary>
        /// Return a single variable
        /// </summary>
        /// <param name="section"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet( "GetSetting" )]
        //public async Task<SettingGetResult> GetSetting(string section, string variable, string buildNumber)
        public IResult GetSetting(string section, string variable, string buildNumber)
        {
            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
            var uid = User.FindFirstValue( UcsClaimTypes.UserId );
            var ver = buildNumber;

            return Results.Ok( ConfigResolver.Instance.GetSetting( env, app, mod, section, variable, ver, uid ) );
            //var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetSetting",
            //                                                     env, section, variable, app, ver.ToInt(), mod, uid );

            //// If the setting is coming from the user row, IsUserOverride = 1 and OverridingUserId = @UserId.
            //// If it’s coming from the shared/ default, IsUserOverride = 0 and OverridingUserId = NULL.

            //DataTable table = dataSet.Tables[0];

            //var retval = new SettingGetResult()
            //{
            //    Variable = variable,
            //    Value = table.Rows[0].GetField("Value",""),
            //    OverridingUserId = table.Rows[0].GetField("OverridingUserId",""),
            //    IsUserOverride = table.Rows[0].GetField("IsUserOverride",false),
            //    EffectiveBuildNumber = table.Rows[0].GetField("EffectiveBuildNumber",""),
            //    EffectiveModule = table.Rows[0].GetField("EffectiveModule",""),

            //};


            //return retval;
        }

        //[Authorize]
        /// <summary>
        /// Sets a single variable
        /// </summary>
        /// <param name="section"></param>
        /// <param name="variable"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost( "SetUserSetting" )]
        [Authorize( Policy = "RequiresSid" )]
        public IResult SetUserSetting( [FromBody] SettingSetRequest body)
        {

            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            //var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
            //var uid = User.FindFirstValue( UcsClaimTypes.UserId );

            // The reason we don't use the Token's user id is because you could be
            // using an editor and updating someone elses account
            //
            return Results.Ok(ConfigResolver.Instance.SetUserSetting( env, app,  body ));

            //var section = body.Section;
            //var variable = body.Variable;
            //var val = body.Value;
            //var ver = body.BuildNumber;            

            //ConfigDatabase.Instance.ExecuteNonQuery("dbo.UCS_SetUserSetting",
            //                                            env, section, variable, app, ver.ToInt(), body.Module, uid, val );

            //return Results.Ok(true);
        }

        /// <summary>
        /// Sets a single default variable
        /// </summary>
        /// <param name="section"></param>
        /// <param name="variable"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost( "SetDefaultSetting" )]
        [Authorize]
        //[Authorize( Policy = "RequiresDefaultAuth")]
        public IResult SetDefaultSetting( [FromBody] SettingSetRequest body)
        {
            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            //var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
            
            var isDefaultAuth = User.FindFirstValue( UcsClaimTypes.IsDefaultAuth )?.ToBool()??false;
            
            return Results.Ok(ConfigResolver.Instance.SetDefaultSetting( env, app, isDefaultAuth, body ));
            
            //var section = body.Section;
            //var variable = body.Variable;
            //var val = body.Value;
            //var ver = body.BuildNumber ;

            ////if (isDefaultAuth )
            //{
            //    ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_SetDefaultSetting",
            //                                                env,
            //                                                section,
            //                                                variable,
            //                                                app,
            //                                                ver.ToInt(),
            //                                                mod,
            //                                                val );
            //    return Results.Ok(true);
            //}
          
        }
        ///// <summary>
        ///// Determines if you can call the default version of methods
        ///// </summary>
        ///// <returns></returns>
        //[Authorize]
        //[HttpGet( "IsDefaultAuthorized" )]
        //public ActionResult<bool> IsDefaultAuthorized()
        //{

        //    var isDefaultAuth = User.FindFirstValue( UcsClaimTypes.IsDefaultAuth );

        //    // I'm expanding this in the CapabilityChecker .. 
        //    //var result = Gist5Database.Instance.ExecuteScalar<bool>(
        //    //"dbo.UCS_IsDefaultAuthorized",
        //    //env, app, mod, sid );

        //    return Ok(isDefaultAuth);
        //}

        /// <summary>
        /// Deletes a NON default variable
        /// </summary>
        /// <param name="section"></param>
        /// <param name="variable"></param>
        /// <param name="buildNumber">passsed in because we want to delete a certain version no any version</param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost( "DeleteUserSetting" )]
        //public async Task DeleteUserSetting(string section, string variable, string buildNumber )
        public async Task DeleteUserSetting([FromBody] SettingSetRequest body)                
        {
            // Delete value (dummy)

            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            //var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
            var uid = User.FindFirstValue( UcsClaimTypes.UserId );


            ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_DeleteUserSetting",
                                                env, body.Section, body.Variable, app,body.BuildNumber.ToInt(), body.Module, uid );
            return;
            
        }

        /// <summary>
        /// Deletes a default variable
        /// </summary>
        /// <param name="section"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost( "DeleteDefaultSetting" )]
        [Authorize( Policy = "RequiresDefaultAuth" )]        
        public async Task DeleteDefaultSetting([FromBody] SettingSetRequest body)
        {
            // In a real implementation, you would delete the default value from a database or configuration store
            var env = User.FindFirstValue( UcsClaimTypes.Enviornment );
            var app = User.FindFirstValue( UcsClaimTypes.Application );
            //var mod = User.FindFirstValue( UcsClaimTypes.Module );
            //var ver = User.FindFirstValue( UcsClaimTypes.BuildNumber );
            //var uid = User.FindFirstValue( UcsClaimTypes.UserId );


            ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_DeleteDefaultSetting",
                                    env, body.Section, body.Variable, app, body.BuildNumber.ToInt(), body.Module );


            return; 
        }


      

    }



}
