using Config.Core;
using Config.Core.Extensions;

using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UnifiedConfiguration.PresentationModel;

namespace UnifiedConfiguration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UnifiedConfigurationViewModel ViewModel
        {
            get => this.DataContext as UnifiedConfigurationViewModel;
            set => this.DataContext = value;
        }

        public static bool IsDesignMode => DesignerProperties.GetIsInDesignMode( new DependencyObject() );

        public MainWindow()
        {
#if DEBUG
            if (!IsDesignMode)
            // Give the REST services time to startup
                System.Threading.Thread.Sleep( 3000 );
#endif
            InitializeComponent();

            var ucvm = new UnifiedConfigurationViewModel();
            ucvm.WindowIcon = UnifiedConfiguration.Properties.Resources.UnifiedConfigurationImage;
            ucvm.WindowTitle = "Unified Configuration";


            

            this.DataContext = ucvm;

            if (!IsDesignMode)
                this.Dispatcher.BeginInvoke( () => ucvm.Setup() );            
        }

        

        
       



    }
}