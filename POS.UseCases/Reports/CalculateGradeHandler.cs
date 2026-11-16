using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.UseCases.Reports;

public sealed class CalculateGradeHandler(
    IStudentRepository studentRepository,
    IRubricRepository rubricRepository,
    GradeCalculationService calculationService,
    GradeStrategyResolver resolver)
{
    public GradeDto Handle(Guid studentId, string modeKey)
    {
        var student = studentRepository.GetById(studentId)
            ?? throw new DomainException("Студент не найден.");
        var strategy = resolver.Resolve(modeKey);
        var criteria = rubricRepository.Get().Criteria;

        var grade = calculationService.Calculate(student, criteria, strategy);
        return new GradeDto(student.Id, student.Name, grade.Value);
    }
}
