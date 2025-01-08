using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartPoultry.Models;
using static SmartPoultry.App;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Organization.xaml
    /// </summary>
    public partial class Organization : UserControl
    {
        public AppDbContext context = new AppDbContext();
        public UserServices userServices;
        public UserLogsServices logServices;
        public Organization()
        {
            InitializeComponent();
            userServices = new UserServices(context);
            logServices = new UserLogsServices(context);
            GetUserList("active");
        }
        public void ViewUser(User user)
        {
            NameLabel.Content = user.Username;
            RoleLabel.Content = user.Role;

            if(UserLogsList.Children.Count > 0)
            {
                UserLogsList.Children.Clear();
            }

            List<UserLogs> userLogs = logServices.GetListOfMember(user.Id);
            int evenodd = 0;
            for (int i = 0; i < userLogs.Count; i++) 
            {
                
                Organization_UserLogs control = new Organization_UserLogs(userLogs[i], evenodd);
                UserLogsList.Children.Add(control);
                if(evenodd == 0)
                {
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
            }
        }
        public void GetUserList(string status)
        {
            if(UserPanel.Children.Count > 0)
            {
                UserPanel.Children.Clear();
            }
            List<User> users = userServices.GetUserList(status);

            int userid = UserContext.CurrentUserId;
            for (int i = 0; i < users.Count; i++) 
            {
                if (users[i].Id == userid || users[i].Id == 1) 
                {
                    continue;
                }
                string name = users[i].Username;
                string role = users[i].Role;
                Organization_UserControl userControl = new Organization_UserControl(users[i], this);
                UserPanel.Children.Add(userControl);
            }
        }
        private void Active_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(ActiveBtn);
            GetUserList("active");
        }

        private void Inactive_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(InactiveBtn);
            GetUserList("inactive");
        }

        public void HandleButtonDesign(Button button)
        {
            SolidColorBrush activecolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2C6E5D"));
            SolidColorBrush inactivecolor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDCEDD5"));
            if (button == ActiveBtn)
            {
                button.Background = activecolor;
                button.BorderBrush = activecolor;
                button.Foreground = Brushes.White;

                InactiveBtn.Foreground = Brushes.Gray;

                InactiveBtn.Background = inactivecolor;
                InactiveBtn.BorderBrush = inactivecolor;
            }
            else if (button == InactiveBtn)
            {
                button.Background = activecolor;
                button.BorderBrush = activecolor;
                button.Foreground = Brushes.White;

                ActiveBtn.Foreground = Brushes.Gray;

                ActiveBtn.Background = inactivecolor;
                ActiveBtn.BorderBrush = inactivecolor;
            }
            else
            {
                button.Background = activecolor;
                button.BorderBrush = activecolor;
                button.Foreground = Brushes.White;

                ActiveBtn.Foreground = Brushes.Gray;
                InactiveBtn.Foreground = Brushes.Gray;

                ActiveBtn.Background = inactivecolor;
                InactiveBtn.Background = inactivecolor;
                ActiveBtn.BorderBrush = inactivecolor;
                InactiveBtn.BorderBrush = inactivecolor;
            }
        }
        
    }
}
