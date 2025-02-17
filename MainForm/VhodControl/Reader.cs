using System;
using Npgsql;
using VhodControl.model;

namespace VhodControl
{
    internal class Reader
    {
        string connString = "Host=localhost;Port=5433;Username=postgres;Password=11299133;Database=postgres";

        public List<Column> ReadColumns ()
        {
            List<Column> columns = new List<Column>(); 

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM public.bun_table", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(new Column(reader.GetString(0),
                                                reader.GetString(1),
                                                reader.GetString(2),
                                                reader.GetString(3)));
                    }
                }
            }

            return columns;
        }

        public List<NonNormalizeData> ReadData ()
        {
            List<NonNormalizeData> nonNormalizeData = new List<NonNormalizeData>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand("select o.order_date,\r\n       o.customer_name,\r\n       o.quantity,\r\n       b.\"name\",\r\n       b.price,\r\n       c.category_name,\r\n       i.ingredient_name,\r\n       r.quantity\r\n  from bun b\r\n  join categorie c\r\n    on c.category_id = b.category_id\r\n  join \"order\" o \r\n    on o.bun_id = b.bun_id\r\n  join recipe r \r\n    on r.bun_id = b.bun_id \r\n  join ingredient i \r\n    on i.ingredient_id = r.ingredient_id ;", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nonNormalizeData.Add(new NonNormalizeData(reader.GetDateTime(0),
                                                                  reader.GetString(1),
                                                                  reader.GetInt32(2),
                                                                  reader.GetString(3),
                                                                  reader.GetDouble(4),
                                                                  reader.GetString(5),
                                                                  reader.GetString(6),
                                                                  reader.GetDouble(7)));
                    }
                }
            }

            return nonNormalizeData;
        }
    }
}
