namespace POS.UseCases.Reports;

public interface IGradeExporter
{
    void Export(IReadOnlyList<GradeDto> grades, string filePath);
}
