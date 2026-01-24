using Config.Core.WPF;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using UnifiedConfiguration.PresentationModel;

namespace UnifiedConfiguration.PresentationModel.Cmd;


   

public class CmdGridDelete : CmdBaseWithCallback, ICommand
{
    public CmdGridDelete()
    {
        this.Text = "Delete";
        this.Key = "X";
        this.KeyModifier = "Control";
        this.GestureText = "Ctrl+X";
    }

    //public override string Text
    //{
    //    get => base.Text;
    //    set => base.Text = value;
    //}

    public override void Update(object obj)
    {
        //CWindowMainViewModel mvvm = (CWindowMainViewModel) obj;
        //this.IsEnabled = mvvm.NodeRoot != null;

    }

    public override void ExecuteSafe(object obj)
    {
        var mvvm = (UnifiedConfigurationViewModel) obj;
        
        mvvm.SelectedRowDelete();

        base.ExecuteSafe( obj );
    }
}