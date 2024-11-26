using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for Supplier_SupplierControl.xaml
    /// </summary>
    public partial class Supplier_SupplierControl : UserControl
    {
        public Supplier_SupplierControl(string name, string contactperson, string contact)
        {

            InitializeComponent();

            Name.Content = name;
            ContactPerson.Content = contactperson;
            ContactInfo.Content = contact;
        }
    }
}
