using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VhodControl.model
{
    public class NonNormalizeData
    {
        public DateTime order_date { get; set; }
        public string customer_name { get; set; }
        public int order_quantity {  get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string category_name { get; set; }
        public string ingredient_name {  get; set; }
        public double recipe_quantity { get; set; }

        public NonNormalizeData(DateTime order_date, string customer_name, int order_quantity, string name,
                                double price, string category_name, string ingredient_name, double recipe_quantity)
        {
            this.order_date = order_date;
            this.customer_name = customer_name;
            this.order_quantity = order_quantity;
            this.name = name;
            this.price = price;
            this.category_name = category_name;
            this.ingredient_name = ingredient_name;
            this.recipe_quantity = recipe_quantity;
        }
    }
}
