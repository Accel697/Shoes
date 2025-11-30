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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        users currentUser;

        public OrderPage(users user)
        {
            InitializeComponent();

            if (user != null)
            {
                currentUser = user;
                tblUser.Text = $"{currentUser.users_roles.title}: {currentUser.last_name} {currentUser.first_name} {currentUser.middle_name}";
            }

            if (currentUser.role == 1) { btnAddOrder.Visibility = Visibility.Visible; } 

            LoadOrder();
        }

        private void LoadOrder()
        {
            using (var context = new shoesEntities1())
            {
                var orders = context.orders.Include("orders_statuses").Include("pick_up_points").OrderByDescending(o => o.order_date).ToList();
                LviewOrders.ItemsSource = orders;
            }
        }

        private void LviewOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentUser.role == 1)
            {
                NavigationService.Navigate(new AddEditOrder(LviewOrders.SelectedItem as orders));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                using (var context = new shoesEntities1())
                {
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    LviewOrders.ItemsSource = null;
                    LoadOrder();
                }
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOrder(null));
        }
    }
}
