namespace VhodControl
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string fileName = "bakery_data.db";
            string PostgresConnectionString = "Host=localhost;Port=5433;Username=postgres;Password=11299133;Database=postgres";
            string jsonName = "input.json";

            string filePath = Path.Combine(exeDirectory, fileName);
            string jsonPath = Path.Combine(exeDirectory, jsonName);

            if (!File.Exists(filePath))
            {
                SQLlite sQlite = new SQLlite();
                sQlite.CreateTable();
                sQlite.InsertData(jsonPath);
                sQlite.MigrateData(PostgresConnectionString);
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}