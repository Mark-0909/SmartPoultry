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

        public int SelectedID;
        public string Role;
        public string Status;

        MainWindow mainWindow;
        public Organization()
        {
            InitializeComponent();
            userServices = new UserServices(context);
            logServices = new UserLogsServices(context);
            GetUserList("active");

            Clear(false);
            SaveChangesBtn.Visibility = Visibility.Hidden;
        }
        public void ViewUser(User user)
        {
            Clear(true);
            NameLabel.Content = user.Username;
            if(user.Role == "admin")
            {
                RoleCBox.SelectedIndex = 0;
            }
            else
            {
                RoleCBox.SelectedIndex = 1;
            }

            SelectedID = user.Id;
            Role = user.Role;
            Status = user.Status;

            mainWindow = UserContext.mainWindow;

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
            SaveChangesBtn.Visibility = Visibility.Hidden;
        }
        public void Clear(bool isVisible)
        {
            if (isVisible)
            {
                NameLabel.Visibility = Visibility.Visible;
                BanBtn.Visibility = Visibility.Visible;
                RoleCBox.Visibility = Visibility.Visible;
                ClearBtn.Visibility = Visibility.Visible;
                
                
            } else 
            {
                NameLabel.Visibility = Visibility.Hidden;
                BanBtn.Visibility = Visibility.Hidden;
                RoleCBox.Visibility = Visibility.Hidden;
                ClearBtn.Visibility = Visibility.Hidden;
                SaveChangesBtn.Visibility = Visibility.Hidden;
                if (UserLogsList.Children.Count != 0)
                {
                    UserLogsList.Children.Clear();
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
            int evenodd = 0;
            for (int i = 0; i < users.Count; i++) 
            {
                if (users[i].Id == userid || users[i].Id == 1) 
                {
                    continue;
                }
                string name = users[i].Username;
                string role = users[i].Role;

                
                Organization_UserControl userControl = new Organization_UserControl(users[i], this, evenodd);

                if(evenodd == 0)
                {
                    evenodd = 1;
                }
                else
                {
                    evenodd = 0;
                }
                UserPanel.Children.Add(userControl);
            }
        }
        private void Active_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(ActiveBtn);
            GetUserList("active");
            Clear(false);
        }

        private void Inactive_Clicked(object sender, RoutedEventArgs e)
        {
            HandleButtonDesign(InactiveBtn);
            GetUserList("inactive");
            Clear(false);
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

        private void BanBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (Status == "active") 
            {
                MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to Ban this User?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                bool isUpdated = userServices.UpdateStatusUser(SelectedID, "ban");
                if (!isUpdated)
                {
                    mainWindow.PopUpNotif("alert", "Ban unsuccessful.");
                    return;
                }
                mainWindow.PopUpNotif("notif", "Ban successfully.");
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to Unban this User?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                bool isUpdated = userServices.UpdateStatusUser(SelectedID, "unban");
                if (!isUpdated)
                {
                    mainWindow.PopUpNotif("alert", "Unban unsuccessful.");
                    return;
                }
                mainWindow.PopUpNotif("notif", "Unban successfully.");
            }
            UserPanel.Children.Clear();
            GetUserList(Status);

        }

        private void RoleCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = RoleCBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null && selectedItem.Content.ToString().Equals(Role, StringComparison.OrdinalIgnoreCase))
            {
                SaveChangesBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                SaveChangesBtn.Visibility = Visibility.Visible;
            }
        }



        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            Clear(false);
        }

        private void SaveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = RoleCBox.SelectedItem as ComboBoxItem;
            string selectedRole = selectedItem?.Content.ToString();

            if (string.IsNullOrEmpty(selectedRole))
            {
                mainWindow.PopUpNotif("alert", "No role selected.");
                return;
            }

            bool isUpdated = userServices.UpdateRole(SelectedID, selectedRole);

            if (!isUpdated)
            {
                mainWindow.PopUpNotif("alert", "Role update unsuccessful.");
                return;
            }

            mainWindow.PopUpNotif("notif", "Role updated successfully.");
        }

    }
}
