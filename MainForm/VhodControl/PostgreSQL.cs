using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VhodControl
{
    internal class PostgreSQL
    {
        public string GenerateSqlQuery(List<string> columns)
        {
            var cols = string.Join(", ", columns);
            return $"""
                   select {cols}
                     from (
                         select distinct
                                o.order_id,
                                b.bun_id,
                                b.name as bun_name,
                                b.price as bun_price,
                                c.category_id,
                                c.category_name,
                                i.ingredient_id,
                                i.ingredient_name,
                                r.quantity as ingredient_quantity,
                                o.order_date,
                                o.customer_name,
                                o.quantity as order_quantity
                           from bun b
                           join category c 
                             on c.category_id = b.category_id 
                           join recipe r 
                             on r.bun_id = b.bun_id 
                           join ingredient i 
                             on i.ingredient_id = r.ingredient_id 
                           join "order" o
                             on o.bun_id = b.bun_id 
                     );
                """;
        }
    }
}
