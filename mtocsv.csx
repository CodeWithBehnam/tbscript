// Updated C# script for Tabular Editor 2 to export all measure descriptions (including table names) to a CSV file.
// This version prompts the user to select the save location using a SaveFileDialog.
// Corrected namespace to System.Windows.Forms (available in Tabular Editor) and fixed dialog result check.

using System.Text;
using System.Windows.Forms;

var sb = new StringBuilder();
sb.AppendLine("\"Table\",\"Measure\",\"Description\"");

foreach (var m in Model.AllMeasures.OrderBy(m => m.Table.Name).ThenBy(m => m.Name))
{
    string table = m.Table.Name;
    string name = m.Name;
    string desc = m.Description ?? "";  // Handle null descriptions as empty

    // Escape quotes in each field
    table = table.Replace("\"", "\"\"");
    name = name.Replace("\"", "\"\"");
    desc = desc.Replace("\"", "\"\"");

    sb.AppendLine("\"" + table + "\",\"" + name + "\",\"" + desc + "\"");
}

var dialog = new SaveFileDialog();
dialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
dialog.FileName = "measures_descriptions.csv";

if (dialog.ShowDialog() == DialogResult.OK)
{
    System.IO.File.WriteAllText(dialog.FileName, sb.ToString());
}
