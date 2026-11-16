using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.UseCases.Evaluations;

public sealed class GetEvaluationsHandler(
    IStudentRepository studentRepository,
    IRubricRepository rubricRepository)
{
    public IReadOnlyList<EvaluationDto> Handle(Guid studentId)
    {
        var student = studentRepository.GetById(studentId)
            ?? throw new DomainException("Студент не найден.");
        var rubric = rubricRepository.Get();

        return student.Evaluations
            .Select(e => EvaluationDto.From(rubric.GetRequired(e.CriterionId), e))
            .ToList();
    }
}
