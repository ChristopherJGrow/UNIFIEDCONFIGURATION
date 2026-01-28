

using Config.Core;
using Config.Core.Extensions;
using Config.Core.Web;

using Models;

using System.Data;
using System.Diagnostics;

using UnifiedConfiguration.Business;

using static Config.Core.DatabaseBase;

namespace UnifiedConfiguration.Test
{
    public sealed class UcsSelfTestScope : IDisposable
    {
        public const string Env = "SelfTest";
        public const string App = "____SelfTestApp";
        public const string Mod = "____SelfTestModule";
        public const string Section = "General";

        public static string UserA { get; } = "S-1-5-21-SELFTEST-USER-A";
        public static string UserB { get; } = "S-1-5-21-SELFTEST-USER-B";

        public UcsSelfTestScope()
        {
            // Clean slate before each test (handles prior failed runs)
            Cleanup();
        }

        public void Cleanup()
        {
            // Defaults in the sproc are SelfTest/App/Module, but pass explicitly if you prefer.
            ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_SelfTestCleanup", Env, App, Mod );
            // If your sproc has defaults and takes no params:
            // ConfigDatabase.Instance.ExecuteNonQuery("dbo.UCS_SelfTestCleanup");
        }

        public void Dispose()
        {
            Cleanup();
        }
    }

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

    public class UnitTest1
    {
       

        [Fact]
        public async Task TestUniqueSections()
        {
            var res = ConfigResolver.Instance;
            var result = res.GetUniqueSections("DEV","IMS");
            Assert.True( result.Sections.Count() > 0 );
        }

        //[Fact]
        //public void Test_Config_TestCleanup()
        //{
        //    ConfigDatabase.Instance.ExecuteNonQuery( "dbo.UCS_SelfTestCleanup" );
        //}

        [Fact]
        public void GetSetting_WhenNoUserOverride_ReturnsDefault()
        {
            using var _ = new UcsSelfTestScope();

            var env = UcsSelfTestScope.Env;
            var app = UcsSelfTestScope.App;
            var mod = UcsSelfTestScope.Mod;
            var section = UcsSelfTestScope.Section;
            var userId = UcsSelfTestScope.UserA;

            var ucs = ConfigResolver.Instance;
            var seed = new UcsTestSeeder(ucs, env, app);

            seed.SetDefault( section, "ApiUrl", "https://default", build: 100, module: mod );

            var got = ucs.GetSetting(env, app, mod, section, "ApiUrl", buildNumber: "100", uid: userId);

            Assert.Equal( "ApiUrl", got.Variable );
            Assert.Equal( "https://default", got.Value );
            Assert.False( got.IsUserOverride );
        }

        [Fact]
        public void GetSetting_WhenUserOverrideExists_UserOverrideWins()
        {
            using var _ = new UcsSelfTestScope();

            var env = UcsSelfTestScope.Env;
            var app = UcsSelfTestScope.App;
            var mod = UcsSelfTestScope.Mod;
            var section = UcsSelfTestScope.Section;
            var userId = UcsSelfTestScope.UserA;

            var ucs = ConfigResolver.Instance;
            var seed = new UcsTestSeeder(ucs, env, app);

            seed.SetDefault( section, "Theme", "Light", build: 1, module: mod );
            seed.SetUser( section, "Theme", "Dark", build: 1, module: mod, userId: userId );

            var got = ucs.GetSetting(env, app, mod, section, "Theme", buildNumber: "1", uid: userId);

            Assert.Equal( "Dark", got.Value );
            Assert.True( got.IsUserOverride );
            Assert.Equal( userId, got.OverridingUserId );
        }

        [Fact]
        public void GetUniqueSections_IncludesSeededSection()
        {
            using var _ = new UcsSelfTestScope();

            var env = UcsSelfTestScope.Env;
            var app = UcsSelfTestScope.App;
            var mod = UcsSelfTestScope.Mod;

            //var section = UcsSelfTestScope.Section;
            var userId = UcsSelfTestScope.UserA;

            var section = "Network";            

            var ucs = ConfigResolver.Instance;
            var seed = new UcsTestSeeder(ucs, env, app);

            seed.SetDefault( section, "X", "1", build: 1, module: mod );

            var sections = ucs.GetUniqueSections(env, app);

            Assert.Contains( "Network", sections.Sections );
        }
    }
}

