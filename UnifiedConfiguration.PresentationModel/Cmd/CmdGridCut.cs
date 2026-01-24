using Config.Core.WPF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnifiedConfiguration.PresentationModel.Cmd
{
    public class CmdGridCut : CmdBaseWithCallback, ICommand
    {

        public CmdGridCut()
        {
            this.Text = "Cut";
            this.Key = "X";
            this.KeyModifier = "Control";
            this.GestureText = "Ctrl+X";
        }
    }
}
