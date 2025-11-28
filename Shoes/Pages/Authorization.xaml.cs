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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void btnEnterAsGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(null));
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new shoesEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login == tbLogin.Text.ToLower().ToString() && u.password == pbPassword.Password.ToString());
                if (user != null)
                {
                    MessageBox.Show("Успешная авторизация", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new MainPage(user));
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    pbPassword.Clear();
                }
            }
        }
    }
}
