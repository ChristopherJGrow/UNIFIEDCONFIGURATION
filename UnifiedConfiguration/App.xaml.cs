using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;

using System.Net;

namespace UnifiedConfiguration
{


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        
        
        protected override async void OnStartup(StartupEventArgs e)
        {           


              
            var main = new MainWindow();
            //{
            //    DataContext = new MainViewModel(auth /* + other services */)
            ///;
            main.Show();
            base.OnStartup( e );
        }
    }

}
