using SmartPoultry.DataAccess;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DbInitializer.Initialize();
        }
        public static class UserContext
        {
            public static int CurrentUserId { get; set; } = -1;
            
            public static MainWindow mainWindow { get; set; }

            public static home homewindow { get; set; }

            
        }

    }
}
