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
using Microsoft.Win32;
using Shoes.Model;

namespace Shoes.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Page
    {
        products currentProduct = new products();

        public AddEditProduct(products product)
        {
            InitializeComponent();
            LoadComboBoxes();

            if (product != null )
            {
                currentProduct = product;
                btnDelete.Visibility = Visibility.Visible;
                tbId.IsEnabled = false;
            }

            currentProduct.unit = 1;

            DataContext = currentProduct;
        }

        private void LoadComboBoxes()
        {
            using (var context = new shoesEntities1())
            {
                var suppliers = context.suppliers.ToList();
                cbSupplier.ItemsSource = suppliers;
                var manufacturers = context.manufacturers.ToList();
                cbManufacturer.ItemsSource = manufacturers;
                var cotegories = context.products_categories.ToList();
                cbCategory.ItemsSource = cotegories;
            }
        }

        private void btnEnterImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog getImageDialog = new OpenFileDialog();

            getImageDialog.Filter = "Файды изображений: (*.png, *.jpg, *.jpeg)| *.png; *.jpg; *.jpeg";
            getImageDialog.InitialDirectory = "C:\\NATK\\implementationandsupport\\Shoes\\Shoes\\Resources\\";
            if (getImageDialog.ShowDialog() == true)
            {
                currentProduct.photo = getImageDialog.SafeFileName;
                img.Source = new BitmapImage(new Uri(getImageDialog.FileName));
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите удалить {currentProduct.title}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new shoesEntities1())
                    {
                        if (!context.orders_products.Any(op => op.product_id == currentProduct.id))
                        {
                            var productInDb = context.products.FirstOrDefault(p => p.id == currentProduct.id);
                            context.products.Remove(productInDb);
                            context.SaveChanges();
                            MessageBox.Show("Запись удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            NavigationService.GoBack();
                        }
                        else
                        {
                            MessageBox.Show("Нельзя удалить продукт, который есть в заказах", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            using (var context = new shoesEntities1())
            {
                if (!context.products.Any(p => p.id == currentProduct.id))
                {
                    try
                    {
                        context.products.Add(currentProduct);
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
                        var productInDb = context.products.FirstOrDefault(p => p.id == currentProduct.id);
                        productInDb.title = currentProduct.title;
                        productInDb.price = currentProduct.price;
                        productInDb.supplier = currentProduct.supplier;
                        productInDb.manufacturer = currentProduct.manufacturer;
                        productInDb.category = currentProduct.category;
                        productInDb.discount = currentProduct.discount;
                        productInDb.quantity_in_stock = currentProduct.quantity_in_stock;
                        productInDb.description = currentProduct.description;
                        productInDb.photo = currentProduct.photo;
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
