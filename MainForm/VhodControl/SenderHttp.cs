using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;  // Убедитесь, что вы подключили нужные пространства имен
using VhodControl.model;

namespace VhodControl
{
    internal class SenderHttp
    {
        private HttpClient client;

        public async void buttonSend_Click(object sender, EventArgs e, DataGridView dataGridView)
        {
            try
            {
                client = new HttpClient();
                var sqlLite = new SQLlite();
                var postgres = new PostgreSQL();

                var columns = sqlLite.GetSelectedColumns(dataGridView);

                if (columns.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы один столбец для отправки.");
                    return;
                }

                string sqlQuery = postgres.GenerateSqlQuery(columns);

                var allData = new List<Dictionary<string, object>>();

                string PostgresConnectionString = "Host=localhost;Port=5433;Username=postgres;Password=11299133;Database=postgres";
                using (var connection = new NpgsqlConnection(PostgresConnectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(sqlQuery, connection))
                    {
                        using (var reader_ = await command.ExecuteReaderAsync())
                        {
                            while (await reader_.ReadAsync())
                            {
                                var rowData = new Dictionary<string, object>();
                                for (int i = 0; i < reader_.FieldCount; i++)
                                {
                                    string columnName = reader_.GetName(i);
                                    object value = reader_.IsDBNull(i) ? null : reader_.GetValue(i);
                                    rowData[columnName] = value;
                                }
                                allData.Add(rowData);
                            }
                        }
                    }
                }

                var requestBody = new
                {
                    SelectedColumns = columns,
                    Rows = allData
                };

                string json = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
                MessageBox.Show("Payload JSON:\n" + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5000/create_excel", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Данные успешно отправлены!");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка при отправке данных. Статус: {response.StatusCode}, Ответ: {responseContent}");
                    Console.WriteLine(responseContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}