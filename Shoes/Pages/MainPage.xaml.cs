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
        users currentUser;

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "По возрастанию",
            "По убыванию"
        };

        public MainPage(users user)
        {
            InitializeComponent();
            LoadComboBoxes();

            if (user != null)
            {
                currentUser = user;
                tblUser.Text = $"{currentUser.users_roles.title}: {currentUser.last_name} {currentUser.first_name} {currentUser.middle_name}";
            }
            else { tblUser.Text = "Пользователь: Гость"; }

            if (currentUser.role == 1 || currentUser.role == 2)
            {
                btnToOrders.Visibility = Visibility.Visible;
                tbSearch.Visibility = Visibility.Visible;
                cbFilter.Visibility = Visibility.Visible;
                cbSort.Visibility = Visibility.Visible;
            }
            if (currentUser.role == 1) { btnAddProduct.Visibility = Visibility.Visible; }

            LoadProduct();
        }

        private void LoadComboBoxes()
        {
            using (var context = new shoesEntities1())
            {
                var suppliers = context.suppliers.ToList();
                suppliers.Add(new suppliers { id = 0, title = "Все поставщики" });
                suppliers = suppliers.OrderBy(s => s.id).ToList();
                cbFilter.ItemsSource = suppliers;

                cbSort.ItemsSource = SortingList;
            }
        }

        private void LoadProduct()
        {
            using (var context = new shoesEntities1())
            {
                var products = context.products.Include("products_categories").Include("suppliers").Include("manufacturers").Include("units").ToList();

                if (cbFilter.SelectedItem is suppliers selectedSupplier && selectedSupplier.id != 0)
                {
                    products = products.Where(p => p.supplier == selectedSupplier.id).ToList();
                }

                if (cbSort.SelectedIndex == 1)
                {
                    products = products.OrderBy(p => p.quantity_in_stock).ToList();
                }

                if (cbSort.SelectedIndex == 2)
                {
                    products = products.OrderByDescending(p => p.quantity_in_stock).ToList();
                }

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
            if (currentUser.role == 1)
            {
                NavigationService.Navigate(new AddEditProduct(LviewProducts.SelectedItem as products));
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProduct(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                using (var context = new shoesEntities1())
                {
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    LviewProducts.ItemsSource = null;
                    LoadProduct();
                }
            }
        }

        private void btnToOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderPage(currentUser));
        }
    }
}
