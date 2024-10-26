using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for home.xaml
    /// </summary>
    public partial class home : UserControl
    {
        public home()
        {
            InitializeComponent();
        }

        //Search function
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //POS Buttons Click Functions
        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveButton(animalAllBtn, animalAllBorder);
            InactiveButton(animalChickenBtn, animalChickenBorder);
            InactiveButton(animalDogBtn, animalDogBorder);
            InactiveButton(animalCatBtn, animalCatBorder);
        }
        private void ChickenButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(animalAllBtn, animalAllBorder);
            ActiveButton(animalChickenBtn, animalChickenBorder);
            InactiveButton(animalDogBtn, animalDogBorder);
            InactiveButton(animalCatBtn, animalCatBorder);
        }
        private void DogButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(animalAllBtn, animalAllBorder);
            InactiveButton(animalChickenBtn, animalChickenBorder);
            ActiveButton(animalDogBtn, animalDogBorder);
            InactiveButton(animalCatBtn, animalCatBorder);
        }
        private void CatButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(animalAllBtn, animalAllBorder);
            InactiveButton(animalChickenBtn, animalChickenBorder);
            InactiveButton(animalDogBtn, animalDogBorder);
            ActiveButton(animalCatBtn, animalCatBorder);
        }
        private void TypeAllButton_Click(object sender, RoutedEventArgs e)
        {
            ActiveButton(typeAllBtn, typeAllBorder);
            InactiveButton(typeFoodBtn, typeFoodBorder);
            InactiveButton(typeToysBtn, typeToysBorder);
            InactiveButton(typeMedsBtn, typeMedsBorder);
        }
        private void FoodButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(typeAllBtn, typeAllBorder);
            ActiveButton(typeFoodBtn, typeFoodBorder);
            InactiveButton(typeToysBtn, typeToysBorder);
            InactiveButton(typeMedsBtn, typeMedsBorder);
        }
        private void ToysButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(typeAllBtn, typeAllBorder);
            InactiveButton(typeFoodBtn, typeFoodBorder);
            ActiveButton(typeToysBtn, typeToysBorder);
            InactiveButton(typeMedsBtn, typeMedsBorder);
        }
        private void MedsButton_Click(object sender, RoutedEventArgs e)
        {
            InactiveButton(typeAllBtn, typeAllBorder);
            InactiveButton(typeFoodBtn, typeFoodBorder);
            InactiveButton(typeToysBtn, typeToysBorder);
            ActiveButton(typeMedsBtn, typeMedsBorder);
        }


        //Active and inactive button properties change
        private void ActiveButton(Button button, Border border)
        {
            border.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(102, 194, 101));
            button.Background = new SolidColorBrush(Color.FromRgb(192, 228, 190));
            button.BorderBrush = new SolidColorBrush(Color.FromRgb(192, 228, 190));
            button.Foreground = new SolidColorBrush(Color.FromRgb(102, 194, 101));
        }
        private void InactiveButton(Button button, Border border)
        {
            border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(243, 243, 243));
            button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            button.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            button.Foreground = new SolidColorBrush(Color.FromRgb(185, 185, 185));
        }
    }
}
