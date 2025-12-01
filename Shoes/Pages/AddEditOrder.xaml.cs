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
using Shoes.Services;

namespace Shoes.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditOrder.xaml
    /// </summary>
    public partial class AddEditOrder : Page
    {
        orders currentOrder = new orders();

        public AddEditOrder(orders order)
        {
            InitializeComponent();
            LoadComboBoxes();

            if (order != null)
            {
                currentOrder = order;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                using (var context = new shoesEntities1())
                {
                    var orderWithMaxCode = context.orders.OrderByDescending(o => o.code).FirstOrDefault();
                    currentOrder.code = orderWithMaxCode.code + 1;

                }
            }

            DataContext = currentOrder;
        }

        private void LoadComboBoxes()
        {
            using (var context = new shoesEntities1())
            {
                var clients = context.users.ToList();
                cbClient.ItemsSource = clients;
                var statuses = context.orders_statuses.ToList();
                cbStatus.ItemsSource = statuses;
                var pickUpPoints = context.pick_up_points.ToList();
                cbPickUpPoint.ItemsSource = pickUpPoints;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите удалить данный заказ?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new shoesEntities1())
                    {
                        if (!context.orders_products.Any(op => op.order_id == currentOrder.id))
                        {
                            var orderInDb = context.orders.FirstOrDefault(o => o.id == currentOrder.id);
                            context.orders.Remove(orderInDb);
                            context.SaveChanges();
                            MessageBox.Show("Запись удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Нельзя удалить заказ, в котором есть в товары", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DataValidator validator = new DataValidator();
            var (isValid, errors) = validator.OrderValidator(currentOrder);

            if (!isValid ) //проверка на валидность
            {
                MessageBox.Show(string.Join("\n", errors), "Ошибки валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new shoesEntities1())
            {
                if (!context.orders.Any(o => o.id == currentOrder.id))
                {
                    try
                    {
                        context.orders.Add(currentOrder);
                        context.SaveChanges();
                        MessageBox.Show("Информация сохранена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        var orderInDb = context.orders.FirstOrDefault(o => o.id == currentOrder.id);
                        orderInDb.client = currentOrder.client;
                        orderInDb.status = currentOrder.status;
                        orderInDb.pick_up_point = currentOrder.pick_up_point;
                        orderInDb.order_date = currentOrder.order_date;
                        orderInDb.delivery_date = currentOrder.delivery_date;
                        context.SaveChanges();
                        MessageBox.Show("Информация сохранена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
