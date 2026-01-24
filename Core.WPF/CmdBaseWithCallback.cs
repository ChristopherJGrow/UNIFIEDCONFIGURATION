using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core.WPF
{
    public abstract class CmdBaseWithCallback : CmdBase, ICommandWithCallback
    {
        public virtual Action<object> Callback { get; set; }

        public override void ExecuteSafe(object obj)
        {
            if (this.Callback != null)
                this.Callback(obj);
        }
    }
}
