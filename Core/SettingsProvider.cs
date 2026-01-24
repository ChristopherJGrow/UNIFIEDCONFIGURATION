
using Config.Core.Conversion;
using Config.Core.Extensions;

using Microsoft.Extensions.Configuration;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static System.Collections.Specialized.BitVector32;

namespace Config.Core
{
    public class SettingsProvider
    {
        protected IConfigurationRoot settings { get; init; } = new ConfigurationBuilder()
                                                                                        .SetBasePath(AppContext.BaseDirectory)
                                                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                                        .AddEnvironmentVariables() // Allow overriding via env vars
                                                                                        .Build();

        private static Lazy<SettingsProvider>       __Instance = new Lazy<SettingsProvider>(()=> new SettingsProvider());
        public static SettingsProvider Instance =>  __Instance.Value;

        //--------------------------------------------------------------------


        public string GetConnectionString(string name)
        {
            return this.settings.GetConnectionString(name);
        }
        public T Get<T>(string variable, T def)
        {
            return ConvertEx.From( this.settings.GetSection( variable ).Value, def );                       
        }
        public T Get<T>(string section, string variable, T def)
        {
            return ConvertEx.From( this.settings.GetSection( section).GetSection(variable).Value, def );
        }
        public T Get<T>(string section1,string section, string variable, T def)
        {
            return ConvertEx.From( this.settings.GetSection( section1 ).GetSection(section).GetSection( variable ).Value, def );
        }

    }
}
