using Config.Core;
using Config.Core.Auth;
using Config.Core.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Models;

using System.Data;

namespace UnifiedConfiguration.Business
{
    public class ConfigResolver
    {
        static Lazy<ConfigResolver> _Instance = new Lazy<ConfigResolver>(() => new ConfigResolver());
        public static ConfigResolver Instance => _Instance.Value;
        public UniqueSectionsResult GetUniqueSections(string env, string app)
        {

            var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetUniqueSections",env, app );
            DataTable table = dataSet.Tables[0];
            var result = table.AsEnumerable()
                                .Select(row => row.GetField("Section", "") )
                                .ToList();

            return new UniqueSectionsResult() { Sections = result };
        }

        public SectionResult GetSectionSettings(string env, string app, string mod, string section, string buildNumber, string uid)
        {           
            var ver = buildNumber; 

            var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetSectionSettings",env, section, app, ver.ToInt(), mod, uid );

            DataTable table = dataSet.Tables[0];
            var result = table
                    .AsEnumerable()
                    .Select(row =>
                    new SettingGetResult()
                    {
                        Variable = row.GetField("Variable","" ),
                        Value = row.GetField("Value", "")   ,
                        OverridingUserId = row.GetField("OverridingUserId",""),
                        EffectiveBuildNumber =  row.GetField("EffectiveBuildNumber","" ),
                        EffectiveModule = row.GetField("EffectiveModule","" ),

                    } );

            return new SectionResult()
            {
                Environment = env,
                Application = app,
                Module = mod,
                Settings = result.ToList()
            };
        }

        /// <summary>
        /// Gets for all builds all users .. basically unfiltered aside from env, app and mod
        /// </summary>
        /// <param name="env"></param>
        /// <param name="app"></param>
        /// <param name="mod"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public SectionResult GetSectionSettingsAllBuilds(string env, string app, string mod, string section)
        {
            var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetSectionSettingsAllBuilds",env, section, app,  mod  );

            DataTable table = dataSet.Tables[0];
            var result = table
                    .AsEnumerable()
                    .Select(row =>
                    new SettingGetResult()
                    {
                        Variable = row.GetField("Variable","" ),
                        Value = row.GetField("Value", "")   ,
                        //OverridingUserId = row.GetField("OverridingUserId",""),
                        EffectiveBuildNumber =  row.GetField("BuildNumber","" ),
                        EffectiveModule = row.GetField("ModuleName","" ),
                        OverridingUserId = row.GetField("UserId","")
                    } );

            return new SectionResult()
            {
                Environment = env,
                Application = app,
                Module = mod,
                Settings = result.ToList()
            };
        }

        public SettingGetResult GetSetting(string env, string app, string mod, string section, string variable, string buildNumber, string uid)
        {
            var ver = buildNumber;

            var dataSet = ConfigDatabase.Instance.ExecuteDataSet("dbo.UCS_GetSetting",
                                                                 env, section, variable, app, ver.ToInt(), mod, uid );

            // If the setting is coming from the user row, IsUserOverride = 1 and OverridingUserId = @UserId.
            // If it’s coming from the shared/ default, IsUserOverride = 0 and OverridingUserId = NULL.
           
            DataTable table = dataSet.Tables[0];
            if (table.Rows.Count > 0)
            {
                return new SettingGetResult()
                {
                    Variable = variable,
                    Value = table.Rows[0].GetField("Value",""),
                    OverridingUserId = table.Rows[0].GetField("OverridingUserId",""),
                    IsUserOverride = table.Rows[0].GetField("IsUserOverride",false),
                    EffectiveBuildNumber = table.Rows[0].GetField("EffectiveBuildNumber",""),
                    EffectiveModule = table.Rows[0].GetField("EffectiveModule",""),

                };
            }
            else
            {
                return new SettingGetResult()
                {
                    Variable = variable,
                    Value = "" ,
                    OverridingUserId = "",
                    IsUserOverride =  false ,
                    EffectiveBuildNumber = "" ,
                    EffectiveModule = "",
                };

            }
            
        }


        public bool SetUserSetting(string env, string app, SettingSetRequest body)
        {
            var section = body.Section;
            var variable = body.Variable;
            var val = body.Value;
            var ver = body.BuildNumber;
            var uid = body.UserId;

            ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_SetUserSetting",
                                                        env,
                                                        section,
                                                        variable,
                                                        app,
                                                        ver.ToInt(),
                                                        body.Module,
                                                        uid,
                                                        val );

            return true;
        }

        public bool SetDefaultSetting(string env, string app,bool isDefaultAuth , [FromBody] SettingSetRequest body)
        {
            var section = body.Section;
            var variable = body.Variable;
            var val = body.Value;
            var ver = body.BuildNumber ;
            var mod = body.Module;

            if (isDefaultAuth )
            {
                ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_SetDefaultSetting",
                                                            env,
                                                            section,
                                                            variable,
                                                            app,
                                                            ver.ToInt(),
                                                            mod,
                                                            val );                
            }
            return isDefaultAuth;
                
        }

    }
}
