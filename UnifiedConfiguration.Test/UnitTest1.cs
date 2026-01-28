

using Config.Core;
using Config.Core.Extensions;
using Config.Core.Web;

using Models;

using System.Data;
using System.Diagnostics;

using UnifiedConfiguration.Business;

using static Config.Core.DatabaseBase;

[assembly: CollectionBehavior( DisableTestParallelization = true )]

namespace UnifiedConfiguration.Test
{


   

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

        [Fact]
        public void GetSetting_BuildNumber_SelectsCorrectBuild()
        {
            using var _ = new UcsSelfTestScope();

            var ucs = ConfigResolver.Instance;
            var seed = new UcsTestSeeder(ucs, UcsSelfTestScope.Env, UcsSelfTestScope.App);

            seed.SetDefault( UcsSelfTestScope.Section, "ApiUrl", "https://b100", build: 100, module: UcsSelfTestScope.Mod );
            seed.SetDefault( UcsSelfTestScope.Section, "ApiUrl", "https://b200", build: 200, module: UcsSelfTestScope.Mod );

            var got100 = ucs.GetSetting(UcsSelfTestScope.Env, UcsSelfTestScope.App, UcsSelfTestScope.Mod,UcsSelfTestScope.Section, "ApiUrl", "100", UcsSelfTestScope.UserA);

            var got200 = ucs.GetSetting(UcsSelfTestScope.Env, UcsSelfTestScope.App, UcsSelfTestScope.Mod,UcsSelfTestScope.Section, "ApiUrl", "200", UcsSelfTestScope.UserA);

            Assert.Equal( "https://b100", got100.Value );
            Assert.Equal( "https://b200", got200.Value );
        }

        [Fact]
        public void GetSectionSettings_ReturnsAllVariablesInSection()
        {
            using var _ = new UcsSelfTestScope();

            var ucs = ConfigResolver.Instance;
            var seed = new UcsTestSeeder(ucs, UcsSelfTestScope.Env, UcsSelfTestScope.App);

            seed.SetDefault( UcsSelfTestScope.Section, "A", "1", build: 1, module: UcsSelfTestScope.Mod );
            seed.SetDefault( UcsSelfTestScope.Section, "B", "2", build: 1, module: UcsSelfTestScope.Mod );

            var section = ucs.GetSectionSettings(UcsSelfTestScope.Env, UcsSelfTestScope.App, UcsSelfTestScope.Mod,
        UcsSelfTestScope.Section, buildNumber: "1", uid: UcsSelfTestScope.UserA);

            Assert.Contains( section.Settings, s => s.Variable == "A" && s.Value == "1" );
            Assert.Contains( section.Settings, s => s.Variable == "B" && s.Value == "2" );
        }

        [Fact]
        public void GetSetting_WhenMissing_ReturnsEmptyOrThrowsPredictably()
        {
            using var _ = new UcsSelfTestScope();

            var ucs = ConfigResolver.Instance;

            // Depending on your desired behavior:
            var got = ucs.GetSetting(UcsSelfTestScope.Env, UcsSelfTestScope.App, UcsSelfTestScope.Mod,
        UcsSelfTestScope.Section, "DoesNotExist", "1", UcsSelfTestScope.UserA);

            // If you return empty:
            Assert.Equal( "", got.Value );
        }




    }
}

