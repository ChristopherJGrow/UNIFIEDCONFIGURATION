using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Config.Core;
using Config.Core.Extensions;
using Config.Core.WPF;


namespace Config.Core.WPF.Behaviors
{

    public static class DataGridBehaviors
    {
        public static readonly DependencyProperty InitializingNewItemCommandProperty =
    DependencyProperty.RegisterAttached(
        "InitializingNewItemCommand",
        typeof(ICommand),
        typeof(DataGridBehaviors),
        new PropertyMetadata(null, OnInitializingNewItemCommandChanged));

        public static void SetInitializingNewItemCommand(DependencyObject obj, ICommand value) =>
            obj.SetValue( InitializingNewItemCommandProperty, value );

        public static ICommand GetInitializingNewItemCommand(DependencyObject obj) =>
            (ICommand) obj.GetValue( InitializingNewItemCommandProperty );

        private static void OnInitializingNewItemCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg)
            {
                dg.InitializingNewItem -= Dg_InitializingNewItem;
                if (e.NewValue is ICommand)
                {
                    dg.InitializingNewItem += Dg_InitializingNewItem;
                }
            }
        }

        private static void Dg_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            var dg = (DataGrid)sender;
            var cmd = GetInitializingNewItemCommand(dg);
            if (cmd?.CanExecute( e.NewItem ) == true)
                cmd.Execute( e.NewItem );
        }
    }

}