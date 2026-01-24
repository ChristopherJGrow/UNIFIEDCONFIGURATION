using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class SettingSetRequest // this is here because it may contain characters not appropirate in a URL if passed as a param
    {
        public string Section { get; set; } = "";
        public string Variable { get; set; } = "";
        public string Value { get; set; } = "";

        public string Module { get; set; } = "";
        public string BuildNumber { get; set; } = "";
        public string UserId { get; set;  } // used when altering other peoples records
        public bool IsDefault { get; set; } = false;

    }
}
