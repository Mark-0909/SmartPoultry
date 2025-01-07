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
        
        public static class UserContext
        {
            public static int CurrentUserId { get; set; } = -1;
            
            public static MainWindow mainWindow { get; set; }
        }

    }
}
