using VhodControl.SQLlite;

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

            string filePath = Path.Combine(exeDirectory, fileName);

            if (!File.Exists(filePath))
            {
                var nonNormalizeTable = new CreateTable();
                nonNormalizeTable.InitiateSQLiteTable();
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}