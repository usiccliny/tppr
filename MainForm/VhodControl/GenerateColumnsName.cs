namespace VhodControl
{
    internal class GenerateColumnsName
    {
        public void LoadColumnNames(DataGridView dataGridViewColumns)
        {
            Reader reader = new Reader();
            var columns = reader.ReadColumns();

            dataGridViewColumns.Rows.Clear();
            dataGridViewColumns.AllowUserToAddRows = false;

            if (dataGridViewColumns.Columns.Count == 0)
            {
                dataGridViewColumns.Columns.Add("ColumnName", "Название колонки");
                dataGridViewColumns.Columns.Add("IncludeInReport", "Включить в отчет");

                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "Button";
                buttonColumn.HeaderText = "Выбор";
                buttonColumn.Text = "Выбрать";
                buttonColumn.UseColumnTextForButtonValue = true;
                dataGridViewColumns.Columns.Add(buttonColumn);

                dataGridViewColumns.Columns[0].Width = 250;
                dataGridViewColumns.Columns[1].Width = 75;
                dataGridViewColumns.Columns[2].Width = 132;
            }

            for (int i = 0; i < columns.Count; i++)
            {
                dataGridViewColumns.Rows.Add(columns[i].description, false);
            }
        }

        public void dataGridViewColumns_CellClick(object sender, DataGridViewCellEventArgs e, DataGridView dataGridViewColumns)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                bool isSelected = (bool)dataGridViewColumns.Rows[e.RowIndex].Cells[1].Value;
                dataGridViewColumns.Rows[e.RowIndex].Cells[1].Value = !isSelected;
            }
        }
    }
}
