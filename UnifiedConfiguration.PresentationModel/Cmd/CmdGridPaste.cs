using Config.Core.WPF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnifiedConfiguration.PresentationModel.Cmd
{
    public class CmdGridPaste : CmdBaseWithCallback, ICommand
    {
        public CmdGridPaste()
        {
            this.Text = "_Paste";
            this.Key = "V";
            this.KeyModifier = "Control";
            this.GestureText = "Ctrl+V";
        }


    }
}
