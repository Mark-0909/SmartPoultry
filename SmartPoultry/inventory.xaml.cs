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

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for inventory.xaml
    /// </summary>
    public partial class inventory : UserControl
    {
        public inventory()
        {
            InitializeComponent();
        }

        private void OpenAddForm_Click(object sender, RoutedEventArgs e)
        {
            Inventory_AddingForm addForm = new Inventory_AddingForm();
            addForm.ShowDialog();
        }
    }
}
