using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoes.Model;

namespace Shoes.Services
{
    public class DataValidator
    {
        public (bool isValid, List<string> errors) ProductValidator(products product)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(product.title) || product.title.Length < 3 || product.title.Length > 60)
                errors.Add("Название должно содержать от 3 до 60 символов");

            if (product.price <= 0 || product.price >= 1000000)
                errors.Add("Цена должна быть от 0 до 100000");

            if (product.supplier == 0)
                errors.Add("Укажите поставщика");

            if (product.manufacturer == 0)
                errors.Add("Укажите производителя");

            if (product.category == 0)
                errors.Add("Укажите категорию");

            if (product.discount != null && (product.discount < 0 || product.discount > 100))
                errors.Add("Скидка должна быть от 1 до 100");

            if (product.quantity_in_stock != null && product.quantity_in_stock < 0)
                errors.Add("Количество на складе не может быть отрицательным");

            return (errors.Count == 0, errors);
        }

        public (bool isValid, List<string> errors) OrderValidator(orders order)
        {
            List<string > errors = new List<string>();

            if (order.order_date == DateTime.MinValue)
                errors.Add("Укажите корректную дату заказа");

            if (order.delivery_date == DateTime.MinValue)
                errors.Add("Укажите корректную дату доставки");

            if (order.pick_up_point == 0)
                errors.Add("Укажите пункт выдачи");

            if (order.client == 0)
                errors.Add("Укажите клиента");

            if (order.status == 0)
                errors.Add("Укажите статус");

            return (errors.Count == 0, errors);
        }
    }
}
