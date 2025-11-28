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
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Page
    {
        products currentProduct;

        public AddEditProduct(products product)
        {
            InitializeComponent();
            if (product != null )
            {
                currentProduct = product;
            }
            else
            {
                tblId.Visibility = Visibility.Visible;
                tbId.Visibility = Visibility.Visible;
            }
            currentProduct.unit = 1;
            DataContext = this;
        }
    }
}
