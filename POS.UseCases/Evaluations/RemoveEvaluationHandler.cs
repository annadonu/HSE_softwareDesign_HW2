using POS.Models;
using POS.UseCases.Students;

namespace POS.UseCases.Evaluations;

public sealed class RemoveEvaluationHandler(IStudentRepository repository)
{
    public void Handle(Guid studentId, Guid criterionId)
    {
        var student = repository.GetById(studentId)
            ?? throw new DomainException("Студент не найден.");
        student.RemoveEvaluation(criterionId);
        repository.Update(student);
    }
}
