// C# script for Tabular Editor 2 to export all measure descriptions (including table names) to a CSV file.
// This script quotes all fields to safely handle commas, quotes, or newlines in names/descriptions.
// When executed in Tabular Editor 2, it will prompt for a save location via SaveFile.

var sb = new System.Text.StringBuilder();
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

SaveFile("measures_descriptions.csv", sb.ToString());
