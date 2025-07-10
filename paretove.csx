using System;
using System.Windows.Forms;

namespace ParetoGenerator
{
    public class ParetoForm : Form
    {
        private Label lblMeasure;
        private ComboBox cmbMeasure;
        private Label lblColumn;
        private ComboBox cmbColumn;
        private Button btnGenerate;
        private TextBox txtDaxOutput;

        public ParetoForm()
        {
            this.Text = "Pareto Measure Generator";
            this.Size = new System.Drawing.Size(500, 400);

            lblMeasure = new Label() { Text = "Select Measure:", Location = new System.Drawing.Point(10, 10) };
            cmbMeasure = new ComboBox() { Location = new System.Drawing.Point(120, 10), Width = 350 };
            // Hardcoded example measures; replace with your actual measures or load dynamically if connected to model
            cmbMeasure.Items.AddRange(new string[] { "Total Sales", "Total Cost", "Profit Margin" });

            lblColumn = new Label() { Text = "Select Column:", Location = new System.Drawing.Point(10, 50) };
            cmbColumn = new ComboBox() { Location = new System.Drawing.Point(120, 50), Width = 350 };
            // Hardcoded example columns; replace with your actual columns (e.g., 'Products'[ProductName])
            cmbColumn.Items.AddRange(new string[] { "'Sales'[Category]", "'Products'[ProductName]", "'Customers'[CustomerID]" });

            btnGenerate = new Button() { Text = "Generate Pareto Measure", Location = new System.Drawing.Point(10, 90), Width = 460 };
            btnGenerate.Click += BtnGenerate_Click;

            txtDaxOutput = new TextBox() { Multiline = true, Location = new System.Drawing.Point(10, 130), Width = 460, Height = 220, ScrollBars = ScrollBars.Vertical };

            this.Controls.Add(lblMeasure);
            this.Controls.Add(cmbMeasure);
            this.Controls.Add(lblColumn);
            this.Controls.Add(cmbColumn);
            this.Controls.Add(btnGenerate);
            this.Controls.Add(txtDaxOutput);
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbMeasure.Text) || string.IsNullOrEmpty(cmbColumn.Text))
            {
                MessageBox.Show("Please select a measure and a column.");
                return;
            }

            string measureName = cmbMeasure.Text;
            string categoryColumn = cmbColumn.Text;

            // Extract table name from column if in format 'Table'[Column]
            string categoryTableName = categoryColumn.Split('[')[0].Trim('\'');
            string categoryColumnName = categoryColumn.Split('[')[1].Trim(']');

            string dax = $@"
VAR Total = CALCULATE([{measureName}], ALL('{categoryTableName}'))
VAR CurrentValue = [{measureName}]
VAR SummaryTable =
    SUMMARIZE(
        ALLSELECTED('{categoryTableName}'),
        '{categoryTableName}'[{categoryColumnName}],
        ""Value"", [{measureName}]
    )
VAR CumulativeSum =
    SUMX(
        FILTER(
            SummaryTable,
            [Value] >= CurrentValue
        ),
        [Value]
    )
RETURN
    DIVIDE(CumulativeSum, Total)
";

            txtDaxOutput.Text = dax;
            txtDaxOutput.AppendText(Environment.NewLine + Environment.NewLine + "Format as percentage in Power BI. Add to your model via Tabular Editor or DAX query.");
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParetoForm());
        }
    }
}
