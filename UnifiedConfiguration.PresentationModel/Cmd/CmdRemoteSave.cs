using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

using Config.Core.WPF;

using UnifiedConfiguration.PresentationModel;

namespace UnifiedConfiguration.PresentationModel.Cmd;


//public class CmdRemoteSave : CmdBaseWithCallback, ICommand
public class CmdRemoteSave : AsyncCmdBase, ICommand
{
    public CmdRemoteSave()
    {
        this.Text = "_Save";
        this.Key = "S";
        this.KeyModifier = "Control";
        this.GestureText = "Ctrl+S";
    }

    public override void Update(object obj)
    {
        //CWindowMainViewModel mvvm = (CWindowMainViewModel) obj;
        //this.IsEnabled = mvvm.NodeRoot != null;

    }


    //protected abstract Task ExecuteSafeAsync(object obj);
    protected override async Task ExecuteSafeAsync(object obj)
    {
        var mvvm = (UnifiedConfigurationViewModel) obj;

        //Task.Run( async ()=>
        //{

            var reloadNeeded= await mvvm.SaveChangesAsync();
            if (reloadNeeded)
                await mvvm.ItemsLoadFromRestToCache();

        //} ).GetAwaiter().GetResult();

        //var thing = Task.Run(()=> mvvm.SaveChangesAsync()).GetAwaiter().GetResult();
        //if (thing)
        //    Task.Run( () => mvvm.ItemsLoadFromRestToCache() ).GetAwaiter().GetResult();

        foreach (var item in mvvm.ItemSource)
        {
            if (item.IsDirty)
                Debug.WriteLine( $"Section={item.Section} Var={item.Variable} Val={item.Value}" );
        }

        //base.ExecuteSafe(obj);

    }
}

