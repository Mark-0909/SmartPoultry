using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using SmartPoultry.Models;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using static SmartPoultry.App;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Add_FinancialLiabilitiesControl.xaml
    /// </summary>
    public partial class Add_FinancialLiabilitiesControl : UserControl
    {
        public int ID;
        public AppDbContext context = new AppDbContext();
        FinancialLiabilitiesServices financialLiabilitiesServices;
        public Add_FinancialLiabilitiesControl(FinancialLiabilities finace, int evenodd)
        {
            InitializeComponent();
            Namelabel.Content = finace.name;
            DueDateLabel.Content = finace.due_date;
            AmountLabel.Content = finace.amount;
            ID = finace.Id;
            financialLiabilitiesServices = new FinancialLiabilitiesServices(context);

            DateTime dueDateValue;
            bool isValidDate = DateTime.TryParse(finace.due_date.ToString(), out dueDateValue);

            if (isValidDate)
            {
                
                DateTime today = DateTime.Now.Date;
                DateTime yellowDate = today.AddDays(1);

                
                if (dueDateValue.Date <= today) 
                {
                    thisBorder1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF3A8A8")); // Red
                }
                else if (dueDateValue.Date == yellowDate)
                {
                    thisBorder1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EEAF")); // yellow
                }
            }
            else
            {
                thisBorder1.BorderBrush = new SolidColorBrush(Colors.Gray); 
            }

            if (evenodd == 1)
            {
                thisBorder1.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                thisBorder1.Background = new SolidColorBrush(Colors.LightGray); 
            }
        }

        private void Info_Clicked(object sender, RoutedEventArgs e)
        {
            FinancialLiabilities var = financialLiabilitiesServices.GetById(ID);
            

            MainWindow? mainWindow = UserContext.mainWindow;

            Add_FinancialLiabilities window = new Add_FinancialLiabilities(var, mainWindow);
            if (mainWindow != null)
            {

                mainWindow.ActiveOverlay(true);
                window.ShowDialog();

            }
            else
            {
                MessageBox.Show("Unable to access the MainWindow. add financial");
            }

            
        }
    }
}
