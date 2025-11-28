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
        long role;

        public MainPage(users user)
        {
            InitializeComponent();
            if (user != null)
            {
                role = user.role;
            }
            if (role == 2 || role == 3)
            {
                tbSearch.Visibility = Visibility.Visible;
                cbFilter.Visibility = Visibility.Visible;
                cbSort.Visibility = Visibility.Visible;
            }
            LoadProduct();
        }

        private void LoadProduct()
        {
            using (var context = new shoesEntities1())
            {
                var products = context.products.Include("products_categories").Include("units").ToList();
                LviewProducts.ItemsSource = products;
            }
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            LoadProduct();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProduct();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProduct();
        }

        private void LviewProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (role == 1)
            {
                NavigationService.Navigate(new AddEditProduct(LviewProducts.SelectedItem as products));
            }
        }
    }
}
