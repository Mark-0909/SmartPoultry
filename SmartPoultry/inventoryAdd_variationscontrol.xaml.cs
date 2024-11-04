using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for inventoryAdd_variationscontrol.xaml
    /// </summary>
    public partial class inventoryAdd_variationscontrol : UserControl
    {
        public string namevar;
        public string pricevar;
        public string conversionvar;
        public string typevar;
        public string stocksvar;
        public string basevaluevar;

        public int positionlist;

        public Inventory_AddingForm? inventoraddingWindow = Application.Current.Windows.OfType<Inventory_AddingForm>().FirstOrDefault();

        public inventoryAdd_variationscontrol(string unitname, string price, string conversion, string type, string stocks, string basevalue, int position)
        {
            InitializeComponent();
            nameBtn.Content = unitname;
            priceLabel.Content = price;
            conversionLabel.Content = conversion;

            namevar = unitname;
            pricevar = price;
            conversionvar = conversion;
            typevar = type;
            stocksvar = stocks;
            basevaluevar = basevalue;

            positionlist = position;
        }
        private void NameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (typevar == "base")
            {
                Inventory_Unitadder popup = new Inventory_Unitadder("base_unit", "edit", namevar, pricevar, conversionvar, basevaluevar, stocksvar, this, positionlist);
                popup.ShowDialog();
            }
            else {
                Inventory_Unitadder popup = new Inventory_Unitadder("sub_unit", "edit", namevar, pricevar, conversionvar, basevaluevar, stocksvar, this, positionlist);
                popup.ShowDialog();
            }
        }

        public void EditUnit(string name, string price, string conversion, string stocks, string baseunit, int position) { 
            
            nameBtn.Content = name;
            priceLabel.Content = price;
            conversionLabel.Content = conversion;

            inventoraddingWindow.UpdateBaseValueForAllInstances(name, price, conversion, stocks, baseunit, position);



        }
    }
}
