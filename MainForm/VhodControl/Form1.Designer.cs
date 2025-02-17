namespace VhodControl
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewColumns = new DataGridView();
            buttonSend = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewColumns).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewColumns
            // 
            dataGridViewColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewColumns.Location = new Point(0, 0);
            dataGridViewColumns.Name = "dataGridViewColumns";
            dataGridViewColumns.Size = new Size(517, 451);
            dataGridViewColumns.TabIndex = 0;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(554, 331);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(234, 107);
            buttonSend.TabIndex = 1;
            buttonSend.Text = "Сформировать отчёт";
            buttonSend.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonSend);
            Controls.Add(dataGridViewColumns);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridViewColumns).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridViewColumns;
        private Button buttonSend;
    }
}
