using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VhodControl.model;

namespace VhodControl
{
    internal class SenderHttp
    {
        readonly string dbFileName = "bakery_data.db";

        private List<string> GetSelectedColumnsFromDataGridView(DataGridView dataGridView)
        {
            List<string> selectedColumns = new List<string>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow && row.Cells[1].Value != null && row.Cells[1].Value is bool && (bool)row.Cells[1].Value)
                {
                    if (row.Cells[0].Value != null)
                    {
                        selectedColumns.Add(row.Cells[0].Value.ToString());
                    }
                }
            }

            return selectedColumns;
        }

        private HttpClient client;

        public async void buttonSend_Click(object sender, EventArgs e, DataGridView dataGridView)
        {
            try
            {
                client = new HttpClient();

                Reader reader = new Reader();
                var columns = reader.ReadColumns();
                string columnList;

                var selectedColumns = GetSelectedColumnsFromDataGridView(dataGridView);

                if (selectedColumns.Count == 0)
                {
                    MessageBox.Show("Выберите хотя бы один столбец для отправки.");
                    return;
                }

                string selectedEntries = string.Join(", ",
                                                    columns.Where(c => selectedColumns.Contains(c.description))
                                                           .Select(c => $"{c.table}_{c.column}")
                                                           .ToList());


                string sqlQuery = $"SELECT {selectedEntries} FROM bakery";

                var allData = new List<Dictionary<string, object>>();

                string connectionString = $"Data Source={dbFileName};Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SQLiteCommand(sqlQuery, connection))
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
                    SelectedColumns = selectedColumns,
                    Rows = allData
                };

                string json = JsonConvert.SerializeObject(requestBody);
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
