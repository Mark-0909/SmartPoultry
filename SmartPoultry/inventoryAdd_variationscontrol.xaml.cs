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
        public Inventory_AddingForm addingform { get; set; }
        public inventoryAdd_variationscontrol(string unitname, string price, string conversion, string type, string stocks, string basevalue, int position, Inventory_AddingForm form)
        {
            InitializeComponent();
            decimal priceconv = decimal.Parse(price);
            nameBtn.Content = unitname;
            priceLabel.Content = $"₱ {priceconv.ToString("N2")}";


            conversionLabel.Content = $"{conversion}/{basevalue}";

            namevar = unitname;
            pricevar = price;
            conversionvar = conversion;
            typevar = type;
            stocksvar = stocks;
            basevaluevar = basevalue;

            positionlist = position;

            addingform = form;

            if(unitname == basevalue)
            {
                conversionTemplate.Visibility = Visibility.Hidden;
            }

            AdjustSizesAndMargins();
        }

        public void UpdateBaseUnit(string BaseUnit)
        {
            basevaluevar = BaseUnit;
            conversionLabel.Content = $"{conversionvar}/{BaseUnit}";
        }

        private void AdjustSizesAndMargins()
        {
            double unitWidth = MeasureContentWidth(nameBtn);
            double priceWidth = MeasureContentWidth(priceLabel);
            double conversionWidth = MeasureContentWidth(conversionLabel);

            double maxWidth = Math.Max(unitWidth, Math.Max(priceWidth, conversionWidth));

            unitTemplate.Width = maxWidth;
            priceTemplate.Width = maxWidth;
            conversionTemplate.Width = maxWidth;

            this.Width = maxWidth + 10;

            var currentMargin = this.Margin;
            this.Margin = new Thickness(-5, currentMargin.Top, 0, currentMargin.Bottom);
        }

        private double MeasureContentWidth(FrameworkElement content)
        {
            if (content != null)
            {
                content.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                return content.DesiredSize.Width + 10; 
            }
            return 0;
        }

        private void NameBtn_Click(object sender, RoutedEventArgs e)
        {
            if (typevar == "base")
            {
                Inventory_Unitadder popup = new Inventory_Unitadder("base_unit", "edit", namevar, pricevar, conversionvar, basevaluevar, stocksvar, this, positionlist, addingform);
                addingform.ActiveOverlay(true);
                popup.ShowDialog();
            }
            else {
                Inventory_Unitadder popup = new Inventory_Unitadder("sub_unit", "edit", namevar, pricevar, conversionvar, basevaluevar, stocksvar, this, positionlist, addingform);
                addingform.ActiveOverlay(true);
                popup.ShowDialog();
            }
        }

        public void EditUnit(string name, string price, string conversion, string stocks, string baseunit, int position)
        {
            nameBtn.Content = name;
            priceLabel.Content = price;
            conversionLabel.Content = conversion;

            inventoraddingWindow.UpdateBaseValueForAllInstances(name, price, conversion, stocks, position);
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            addingform.ClearVariation(positionlist);
        }

        private void UserControl_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (typevar == "base")
            {
                Inventory_Unitadder popup = new Inventory_Unitadder("base_unit", "edit", namevar, pricevar, conversionvar, basevaluevar, stocksvar, this, positionlist, addingform);
                addingform.ActiveOverlay(true);
                popup.ShowDialog();
            }
            else
            {
                Inventory_Unitadder popup = new Inventory_Unitadder("sub_unit", "edit", namevar, pricevar, conversionvar, basevaluevar, stocksvar, this, positionlist, addingform);
                addingform.ActiveOverlay(true);
                popup.ShowDialog();
            }
        }
    }
}
