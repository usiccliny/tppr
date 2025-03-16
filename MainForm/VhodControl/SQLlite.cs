using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Data.SQLite;
using VhodControl.model;

namespace VhodControl
{
    internal class SQLlite
    {
        readonly string dbFileName = "bakery_data.db";

        public void CreateTable()
        {
            string connectionString = $"Data Source={dbFileName};Version=3;";

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS bakery (
                order_id INTEGER NOT NULL,
                bun_id INTEGER NOT NULL,
                bun_name TEXT NOT NULL,
                bun_price REAL NOT NULL,
                category_id INTEGER,
                category_name TEXT NOT NULL,
                ingredient_id INTEGER,
                ingredient_name TEXT NOT NULL,
                ingredient_quantity REAL NOT NULL,
                order_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                customer_name TEXT NOT NULL,
                order_quantity INTEGER NOT NULL
            );";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Таблица bakery успешно создана.");
        }

        public void GenerateColumnNameForDatagrid (DataGridView dataGridView)
        {
            string connectionString = $"Data Source={dbFileName};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "PRAGMA table_info(bakery)";
                using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Columns.Add("Column", typeof(string));
                    dataTable.Columns.Add("Upload", typeof(bool));

                    while (reader.Read())
                    {
                        string columnName = reader["name"].ToString();
                        dataTable.Rows.Add(columnName, false);
                    }

                    dataGridView.DataSource = dataTable;
                }
            }
        }

        public List<string> GetSelectedColumns(DataGridView dataGrid)
        {
            List<string> selectedColumns = new List<string>();
            foreach (DataGridViewRow row in dataGrid.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Upload"].Value))
                {
                    selectedColumns.Add(row.Cells["Column"].Value.ToString());
                }
            }
            return selectedColumns;
        }

        public void InsertData(string jsonData)
        {
            if (!File.Exists(jsonData))
            {
                MessageBox.Show("Файл JSON не найден.");
                return;
            }

            string jsonContent = File.ReadAllText(jsonData);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                MessageBox.Show("Файл JSON пуст.");
                return;
            }

            List<Bakery> bakery;
            try
            {
                bakery = JsonConvert.DeserializeObject<List<Bakery>>(jsonContent);
            }
            catch (JsonReaderException ex)
            {
                MessageBox.Show($"Ошибка десериализации JSON: {ex.Message}");
                return;
            }
            string connectionString = $"Data Source={dbFileName};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                foreach (var order in bakery)
                {
                    var command = new SQLiteCommand(connection)
                    {
                        CommandText = @"
                        INSERT INTO bakery (order_id, bun_id, bun_name, bun_price, category_id, 
                                            category_name, ingredient_id, ingredient_name, 
                                            ingredient_quantity, order_date, customer_name, 
                                            order_quantity) 
                        VALUES (@orderId, @bunId, @bunName, @bunPrice, @categoryId, 
                                @categoryName, @ingredientId, @ingredientName, 
                                @ingredientQuantity, @orderDate, @customerName, 
                                @orderQuantity)"
                    };

                    command.Parameters.AddWithValue("@orderId", order.orderId);
                    command.Parameters.AddWithValue("@bunId", order.bunId);
                    command.Parameters.AddWithValue("@bunName", order.bunName);
                    command.Parameters.AddWithValue("@bunPrice", order.bunPrice);
                    command.Parameters.AddWithValue("@categoryId", order.categoryId);
                    command.Parameters.AddWithValue("@categoryName", order.categoryName);
                    command.Parameters.AddWithValue("@ingredientId", order.ingredientId);
                    command.Parameters.AddWithValue("@ingredientName", order.ingredientName);
                    command.Parameters.AddWithValue("@ingredientQuantity", order.ingredientQuantity);
                    command.Parameters.AddWithValue("@orderDate", order.orderDate);
                    command.Parameters.AddWithValue("@customerName", order.customerName);
                    command.Parameters.AddWithValue("@orderQuantity", order.orderQuantity);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

        }

        public void MigrateData(string pgConString)
        {
            string connectionString = $"Data Source={dbFileName};Version=3;";
            List<Bakery> bakeryList = new List<Bakery>();

            // Считаем все данные из SQLite в список объектов Bakery
            using (var sqliteConnection = new SQLiteConnection(connectionString))
            {
                sqliteConnection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM bakery", sqliteConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bakery bakery = new Bakery
                            {
                                orderId = reader.GetInt32(reader.GetOrdinal("order_id")),
                                bunId = reader.GetInt32(reader.GetOrdinal("bun_id")),
                                bunName = reader.GetString(reader.GetOrdinal("bun_name")),
                                bunPrice = reader.GetDecimal(reader.GetOrdinal("bun_price")),
                                categoryId = reader.GetInt32(reader.GetOrdinal("category_id")),
                                categoryName = reader.GetString(reader.GetOrdinal("category_name")),
                                ingredientId = reader.GetInt32(reader.GetOrdinal("ingredient_id")),
                                ingredientName = reader.GetString(reader.GetOrdinal("ingredient_name")),
                                ingredientQuantity = reader.GetDecimal(reader.GetOrdinal("ingredient_quantity")),
                                orderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                                customerName = reader.GetString(reader.GetOrdinal("customer_name")),
                                orderQuantity = reader.GetInt32(reader.GetOrdinal("order_quantity"))
                            };

                            bakeryList.Add(bakery);
                        }
                    }
                }
            }

            // Загружаем данные в PostgreSQL
            using (var postgresConnection = new NpgsqlConnection(pgConString))
            {
                postgresConnection.Open();

                // Загрузка категорий
                var categoryIds = new Dictionary<string, int>();
                foreach (var bakery in bakeryList)
                {
                    if (!categoryIds.ContainsKey(bakery.categoryName))
                    {
                        using (var categoryCommand = new NpgsqlCommand("INSERT INTO category (category_name) VALUES (@name) ON CONFLICT (category_name) DO NOTHING RETURNING category_id;", postgresConnection))
                        {
                            categoryCommand.Parameters.AddWithValue("name", bakery.categoryName);
                            var newCategoryId = categoryCommand.ExecuteScalar();

                            if (newCategoryId != null)
                            {
                                categoryIds[bakery.categoryName] = (int)newCategoryId;
                            }
                        }
                    }
                }

                // Загрузка ингредиентов
                var ingredientIds = new Dictionary<string, int>();
                foreach (var bakery in bakeryList)
                {
                    if (!ingredientIds.ContainsKey(bakery.ingredientName))
                    {
                        using (var ingredientCommand = new NpgsqlCommand("INSERT INTO ingredient (ingredient_name) VALUES (@name) ON CONFLICT (ingredient_name) DO NOTHING RETURNING ingredient_id;", postgresConnection))
                        {
                            ingredientCommand.Parameters.AddWithValue("name", bakery.ingredientName);
                            var newIngredientId = ingredientCommand.ExecuteScalar();

                            if (newIngredientId != null)
                            {
                                ingredientIds[bakery.ingredientName] = (int)newIngredientId;
                            }
                        }
                    }
                }

                // Загрузка булочек
                var bunIds = new Dictionary<(string, decimal), int>(); // (name, price) -> bun_id
                foreach (var bakery in bakeryList)
                {
                    var bunKey = (bakery.bunName, bakery.bunPrice);
                    if (!bunIds.ContainsKey(bunKey))
                    {
                        using (var bunCommand = new NpgsqlCommand("INSERT INTO bun (name, price, category_id) VALUES (@name, @price, @category_id) ON CONFLICT (name, price) DO NOTHING RETURNING bun_id;", postgresConnection))
                        {
                            bunCommand.Parameters.AddWithValue("name", bakery.bunName);
                            bunCommand.Parameters.AddWithValue("price", bakery.bunPrice);
                            bunCommand.Parameters.AddWithValue("category_id", categoryIds[bakery.categoryName]);
                            var newBunId = bunCommand.ExecuteScalar();

                            if (newBunId != null)
                            {
                                bunIds[bunKey] = (int)newBunId;
                            }
                        }
                    }
                }

                // Загрузка рецептов
                foreach (var bakery in bakeryList)
                {
                    var bunKey = (bakery.bunName, bakery.bunPrice);
                    int bunIdToInsert = bunIds[bunKey];
                    int ingredientIdToInsert = ingredientIds[bakery.ingredientName];

                    using (var recipeCommand = new NpgsqlCommand("INSERT INTO recipe (bun_id, ingredient_id, quantity) VALUES (@bun_id, @ingredient_id, @quantity) ON CONFLICT (bun_id, ingredient_id) DO NOTHING;", postgresConnection))
                    {
                        recipeCommand.Parameters.AddWithValue("bun_id", bunIdToInsert);
                        recipeCommand.Parameters.AddWithValue("ingredient_id", ingredientIdToInsert);
                        recipeCommand.Parameters.AddWithValue("quantity", bakery.ingredientQuantity);
                        recipeCommand.ExecuteNonQuery();
                    }
                }

                // Загрузка заказов
                foreach (var bakery in bakeryList)
                {
                    var bunKey = (bakery.bunName, bakery.bunPrice);
                    int bunIdToInsert = bunIds[bunKey];

                    using (var orderCommand = new NpgsqlCommand("INSERT INTO \"order\" (bun_id, order_date, customer_name, quantity) VALUES (@bun_id, @order_date, @customer_name, @quantity) ON CONFLICT (bun_id, order_date, customer_name, quantity) DO NOTHING;", postgresConnection))
                    {
                        orderCommand.Parameters.AddWithValue("bun_id", bunIdToInsert);
                        orderCommand.Parameters.AddWithValue("order_date", bakery.orderDate);
                        orderCommand.Parameters.AddWithValue("customer_name", bakery.customerName);
                        orderCommand.Parameters.AddWithValue("quantity", bakery.orderQuantity);
                        orderCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
