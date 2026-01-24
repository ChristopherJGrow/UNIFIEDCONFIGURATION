using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Models
{
    [DebuggerDisplay("Environment={Environment}, Application={Application}, Module={Module}, SettingsCount={Settings.Count}")]
    public class SectionResult
    {
        public string Environment { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;

        // Variable, Value, OverridingUserId,EffectiveBuildNumber
        //public List<Tuple<string ,string,string, string >> Settings { get; set; } = new List<Tuple<string, string,string, string>>();

        public List<SettingGetResult> Settings { get; set; } = new List<SettingGetResult>();

    }

    [DebuggerDisplay("Variable={Variable}, Value={Value}, IsUserOverride={IsUserOverride}, OverridingUserId={OverridingUserId}, EffectiveBuildNumber={EffectiveBuildNumber}, EffectiveModule={EffectiveModule}")]

    public class SettingGetResult
    {
        public string   Variable { get; set; } = string.Empty;
        public string   Value { get; set; } = string.Empty;
        public bool     IsUserOverride { get; set; } = false;
        public string   OverridingUserId { get; set; } = string.Empty;
        public string   EffectiveBuildNumber { get; set; } = string.Empty;
        public string   EffectiveModule { get; set; } = string.Empty;        
        //public string   UserId { get; set; } = string.Empty;

    }





}
