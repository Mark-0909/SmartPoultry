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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Inventory_Unitadder.xaml
    /// </summary>
    public partial class Inventory_Unitadder : Window
    {
        public Inventory_AddingForm? inventoraddingWindow = Application.Current.Windows.OfType<Inventory_AddingForm>().FirstOrDefault();

        public static String? actionDeclaration; // add or edit
        public static String? agenda; // base_unit or sub_unit
        public static String? editname;
        public static String? editprice;
        public static String? editconversion;
        public static String? editstocks;
        public static String? baseunit;
        public static int positionlist;


        public static inventoryAdd_variationscontrol? unitcontrol;




        public Inventory_Unitadder(String mode, String baseUnitValue, String action, int position)
        {
            InitializeComponent();
            SetRoundedCorners();
            inventoraddingWindow.Opacity = 0.5;


            
            InitializeUnitAdder(mode, action, null, null, null, baseUnitValue, null, null, position);

            

            this.Closed += (s, e) => inventoraddingWindow.Opacity = 1.0;
        }

        public Inventory_Unitadder(string mode, string action, string name, string price, string conversion, string baseUnitvalue, string stocks, inventoryAdd_variationscontrol control, int position)
        {
            InitializeComponent();
            SetRoundedCorners();
            inventoraddingWindow.Opacity = 0.5;

            
            InitializeUnitAdder(mode, action, name, price, conversion, baseUnitvalue, stocks, control, position);

            

            this.Closed += (s, e) => inventoraddingWindow.Opacity = 1.0;
        }

        // combined methods
        private void InitializeUnitAdder(string? mode, string? action, string? name, string? price, string? conversion, string? baseUnitvalue, string? stocks, inventoryAdd_variationscontrol control, int position)
        {
            
            actionDeclaration = action; 
            agenda = mode;

            unitcontrol = control;

            positionlist = position;

            baseunit = baseUnitvalue;
           
            if (agenda == "base_unit")
            {
                if (actionDeclaration == "add")
                {
                    modeLabel.Content = "Add Base Unit";
                }
                else {
                    modeLabel.Content = "Edit Base Unit";
                }
                

                
                conversionName.Visibility = Visibility.Collapsed;
                conversionLabel.Visibility = Visibility.Collapsed;
                conversionBorder.Visibility = Visibility.Collapsed;
                conversionTextbox.Visibility = Visibility.Collapsed;

                
                if (name != null) unitLabelTextbox.Text = name;
                if (price != null) priceTextBox.Text = price;
                if (stocks != null) stocksTextBox.Text = stocks;
            }
            else if (agenda == "sub_unit")
            {
                if (actionDeclaration == "add")
                {
                    modeLabel.Content = "Add Sub Unit";
                }
                else
                {
                    modeLabel.Content = "Edit Sub Unit";
                }


                stocksBorder.Visibility = Visibility.Collapsed;
                stocksName.Visibility = Visibility.Collapsed;
                stocksTextBox.Visibility = Visibility.Collapsed;

                
                conversionName.Visibility = Visibility.Visible;

                // Set sub unit values
                if (name != null) unitLabelTextbox.Text = name;
                if (baseUnitvalue != null) conversionName.Content = "/ " + baseUnitvalue;
                if (price != null) priceTextBox.Text = price;
                if (conversion != null) conversionTextbox.Text = conversion;
            }
        }



        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (actionDeclaration == "add")
            {
                if (agenda == "base_unit")
                {
                    inventoraddingWindow.AddUnit(unitLabelTextbox.Text, priceTextBox.Text, "1", stocksTextBox.Text, "base", positionlist);
                    this.Close();
                    
                    
                }
                else
                {
                    inventoraddingWindow.AddUnit(unitLabelTextbox.Text, priceTextBox.Text, conversionTextbox.Text, null, "sub", positionlist);
                    this.Close();
                    
                }
            }
            else 
            {
                if (agenda == "base_unit")
                {
                    unitcontrol.EditUnit(unitLabelTextbox.Text, priceTextBox.Text, "1", stocksTextBox.Text, baseunit, positionlist);
                    this.Close();
                    
                    
                }
                else
                {
                    unitcontrol.EditUnit(unitLabelTextbox.Text, priceTextBox.Text, conversionTextbox.Text, null, baseunit, positionlist);

                    this.Close();
                    
                    
                }
            }

        }
        private void SetRoundedCorners()
        {
            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        

    }
}
