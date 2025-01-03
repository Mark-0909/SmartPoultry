using SmartPoultry.DataAccess;
using SmartPoultry.DataServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_AddSupplier.xaml
    /// </summary>
    public partial class Inventory_AddSupplier : Window
    {
        public Inventory_AddingForm addingForm;
        public AppDbContext context = new AppDbContext();
        SupplierServices supplierServices;
        public Inventory_AddSupplier(Inventory_AddingForm addringform)
        {
            InitializeComponent();
            addingForm = addringform;
            supplierServices = new SupplierServices(context);
        }

        private void CloseAddPopup_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
        public void CloseWindow()
        {
            this.Close();
            addingForm.ActiveOverlay(false);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            bool validated = Validation();
            if (!validated) 
            {
                MessageBox.Show("Incomplete Details");
                return;
            }
            bool isCreate = supplierServices.Create(SupplierNameTB.Text, ContactPersonTB.Text, ContactTB.Text, EmailTB.Text, LocationTB.Text);
            if (!isCreate) 
            {
                MessageBox.Show("Unsuccessful.");
                return;
            }

            MessageBox.Show("Successful.");
            addingForm.PopulateSupplierList("add");
            string supplierName = SupplierNameTB.Text;
            if (!string.IsNullOrEmpty(supplierName) && addingForm.SupplierCBox.Items.Contains(supplierName))
            {
                addingForm.SupplierCBox.SelectedItem = supplierName;
            }
            else
            {
                addingForm.SupplierCBox.SelectedIndex = 0;
            }
            CloseWindow();
        }

        public bool Validation()
        {
            if (SupplierNameTB.Text == SupplierNameTB.Tag.ToString() || ContactPersonTB.Text == ContactPersonTB.Tag.ToString() || ContactTB.Text == ContactTB.Tag.ToString() || EmailTB.Text == EmailTB.Tag.ToString() || LocationTB.Text == LocationTB.Tag.ToString())
            {
                return false;
            }
            return true;
        }
        private void TB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholder = textBox.Tag?.ToString();
                if (!string.IsNullOrEmpty(placeholder) && textBox.Text == placeholder)
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void TB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholder = textBox.Tag?.ToString();
                if (!string.IsNullOrEmpty(placeholder) && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.Foreground = Brushes.Gray;
                }
            }
        }
    }
}
