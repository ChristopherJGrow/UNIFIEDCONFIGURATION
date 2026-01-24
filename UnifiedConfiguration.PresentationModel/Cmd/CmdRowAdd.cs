using Config.Core.WPF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnifiedConfiguration.PresentationModel.Cmd
{

    public class CmdRowAdd : CmdBaseWithCallback, ICommand
    {
        public CmdRowAdd()
        {
            this.Text = "_AddRow";
            this.Key = "A";
            this.KeyModifier = "Control";
            this.GestureText = "Ctrl+A";
        }
    }
}
