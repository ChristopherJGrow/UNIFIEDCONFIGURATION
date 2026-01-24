using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Config.Core.Extensions
{
    public static class StopWatchExtension
    {
        public static Stopwatch Time(this Stopwatch sw, Action func)
        {
            
            sw.Start();
            func();
            sw.Stop();
            return sw;
        }
    }
}
