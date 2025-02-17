using System.Data.SQLite;
using VhodControl.model;

namespace VhodControl.SQLlite
{
    public class SQLliteInsert
    {
        readonly string dbFileName = "bakery_data.db";

        public void InsertData()
        {
            string connectionString = $"Data Source={dbFileName};Version=3;";

            var reader = new Reader();
            var columns = reader.ReadColumns();
            List<NonNormalizeData> data = reader.ReadData();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (var item in data)
                {
                    string insertQuery = "INSERT INTO bakery (" + string.Join(", ", columns.Select(c => $"{c.table}_{c.column}")) + ") VALUES (" +
                                         string.Join(", ", columns.Select(c => "@" + $"{c.table}_{c.column}")) + ");";

                    using (var cmd = new SQLiteCommand(insertQuery, connection))
                    {
                        foreach (var column in columns)
                        {
                            string paramName = $"{column.table}_{column.column}";
                            object parameterValue = GetValueForColumn(item, column);

                            cmd.Parameters.AddWithValue("@" + paramName, parameterValue);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private object GetValueForColumn(NonNormalizeData item, Column column)
        {
            return column.column switch
            {
                "order_date" => item.order_date,
                "customer_name" => item.customer_name,
                "quantity" => item.order_quantity,
                "name" => item.name,
                "price" => item.price,
                "category_name" => item.category_name,
                "ingredient_name" => item.ingredient_name,
                "recipe_quantity" => item.recipe_quantity,
                _ => throw new ArgumentException($"Unknown column: {column.column}")
            };
        }
    }
}
