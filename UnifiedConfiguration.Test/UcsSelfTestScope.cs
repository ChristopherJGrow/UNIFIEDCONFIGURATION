using System;
using System.Collections.Generic;
using System.Text;

using UnifiedConfiguration.Business;

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
}
