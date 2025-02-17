using System.Data.SQLite;

namespace VhodControl.SQLlite
{
    internal class CreateTable
    {
        readonly string dbFileName = "bakery_data.db";

        public void InitiateSQLiteTable()
        {
            Reader reader = new Reader();
            var columns = reader.ReadColumns();

            string connectionString = $"Data Source={dbFileName};Version=3;";
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS bakery (";

            List<string> columnDefinitions = new List<string>();
            foreach (var column in columns)
            {
                columnDefinitions.Add($"{column.table}_{column.column} {column.data_type}");
            }

            createTableQuery += string.Join(", ", columnDefinitions);

            createTableQuery += $", PRIMARY KEY ({string.Join(", ", columns.Select(c => $"{c.table}_{c.column}"))})";

            createTableQuery += ");";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Таблица bakery успешно создана.");

            var nonNormalizeData = new SQLliteInsert();
            nonNormalizeData.InsertData();
        }
    }
}
