using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.UseCases.Evaluations;

public sealed class SetEvaluationHandler(
    IStudentRepository studentRepository,
    IRubricRepository rubricRepository,
    EvaluationFactory factory)
{
    public EvaluationDto Handle(SetEvaluationRequest request)
    {
        var student = studentRepository.GetById(request.StudentId)
            ?? throw new DomainException("Студент не найден.");
        var rubric = rubricRepository.Get();

        var evaluation = factory.Create(rubric, request.CriterionId, request.Score, request.Comment);
        student.AddEvaluation(evaluation);
        studentRepository.Update(student);

        return EvaluationDto.From(rubric.GetRequired(evaluation.CriterionId), evaluation);
    }
}
