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
using Shoes.Model;

namespace Shoes.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(users user)
        {
            InitializeComponent();
            LoadProduct();
        }

        private void LoadProduct()
        {
            using (var context = new shoesEntities())
            {
                var products = context.products.ToList();
                LviewProducts.ItemsSource = products;
            }
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LviewProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
