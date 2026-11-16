using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.UseCases.Reports;

public sealed class ExportGradesHandler(
    IStudentRepository studentRepository,
    IRubricRepository rubricRepository,
    GradeCalculationService calculationService,
    GradeStrategyResolver resolver,
    IGradeExporter exporter)
{
    public void Handle(string filePath, string modeKey)
    {
        var strategy = resolver.Resolve(modeKey);
        var criteria = rubricRepository.Get().Criteria;

        var grades = studentRepository.GetAll()
            .Select(student =>
            {
                var grade = calculationService.Calculate(student, criteria, strategy);
                return new GradeDto(student.Id, student.Name, grade.Value);
            })
            .ToList();

        exporter.Export(grades, filePath);
    }
}
