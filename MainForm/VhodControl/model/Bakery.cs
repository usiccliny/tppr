using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VhodControl.model
{
    public class Bakery
    {
        public int orderId { get; set; }
        public int bunId { get; set; }
        public string bunName { get; set; }
        public decimal bunPrice { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public int ingredientId { get; set; }
        public string ingredientName { get; set; }
        public decimal ingredientQuantity { get; set; }
        public DateTime orderDate { get; set; }
        public string customerName { get; set; }
        public int orderQuantity { get; set; }

        public Bakery(int orderId, int bunId, string bunName, decimal bunPrice, int categoryId, string categoryName,
                 int ingredientId, string ingredientName, decimal ingredientQuantity, DateTime orderDate,
                 string customerName, int orderQuantity)
        {
            orderId = orderId;
            bunId = bunId;
            bunName = bunName;
            bunPrice = bunPrice;
            categoryId = categoryId;
            categoryName = categoryName;
            ingredientId = ingredientId;
            ingredientName = ingredientName;
            ingredientQuantity = ingredientQuantity;
            orderDate = orderDate;
            customerName = customerName;
            orderQuantity = orderQuantity;
        }

        public Bakery() { }
    }
    
}
