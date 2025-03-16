namespace VhodControl
{
    public partial class Form1 : Form
    {
        private PythonInitializer pythonInitializer;

        public Form1()
        {
            pythonInitializer = new PythonInitializer();
            _ = pythonInitializer.RunPythonScriptAsync("shaper1.py");

            var sqlLite = new SQLlite();
            var sender = new SenderHttp();

            InitializeComponent();

            sqlLite.GenerateColumnNameForDatagrid(this.dataGridViewColumns);

            this.buttonSend.Click += (s, e) => sender.buttonSend_Click(s, e, this.dataGridViewColumns);

            this.FormClosing += MainForm_FormClosing;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pythonInitializer.StopPythonScript();
        }
    }
}
