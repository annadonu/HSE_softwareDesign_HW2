using System.Globalization;
using System.Text;
using POS.UseCases.Reports;

namespace POS.Infrastructure.Export;

internal sealed class CsvGradeExporter : IGradeExporter
{
    public void Export(IReadOnlyList<GradeDto> grades, string filePath)
    {
        var lines = new List<string> { "StudentId,StudentName,Grade" };

        foreach (var grade in grades)
        {
            var value = grade.Grade.ToString("0.00", CultureInfo.InvariantCulture);
            lines.Add($"{grade.StudentId},{Escape(grade.StudentName)},{value}");
        }

        File.WriteAllLines(filePath, lines, new UTF8Encoding(true));
    }

    private static string Escape(string value) =>
        "\"" + value.Replace("\"", "\"\"") + "\"";
}
