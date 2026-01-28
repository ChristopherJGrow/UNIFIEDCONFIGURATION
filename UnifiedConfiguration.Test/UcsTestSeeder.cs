using Models;

using System;
using System.Collections.Generic;
using System.Text;

using UnifiedConfiguration.Business;

namespace UnifiedConfiguration.Test
{
    public sealed class UcsTestSeeder
    {
        private readonly ConfigResolver _ucs;
        private readonly string _env;
        private readonly string _app;

        public UcsTestSeeder(ConfigResolver ucs, string env, string app)
        {
            _ucs = ucs;
            _env = env;
            _app = app;
        }

        public void SetDefault(string section, string variable, string value, int build, string module)
        {
            _ucs.SetDefaultSetting( _env, _app, isDefaultAuth: true, new SettingSetRequest
            {
                Section = section,
                Variable = variable,
                Value = value,
                BuildNumber = build.ToString(),
                Module = module
            } );
        }

        public void SetUser(string section, string variable, string value, int build, string module, string userId)
        {
            _ucs.SetUserSetting( _env, _app, new SettingSetRequest
            {
                Section = section,
                Variable = variable,
                Value = value,
                BuildNumber = build.ToString(),
                Module = module,
                UserId = userId
            } );
        }
    }
}
