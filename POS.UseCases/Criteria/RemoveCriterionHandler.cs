using POS.Models;
using POS.UseCases.Students;

namespace POS.UseCases.Criteria;

public sealed class RemoveCriterionHandler(
    IRubricRepository rubricRepository,
    IStudentRepository studentRepository)
{
    public void Handle(Guid id)
    {
        var rubric = rubricRepository.Get();
        rubric.GetRequired(id);

        var hasEvaluations = studentRepository.GetAll()
            .Any(s => s.GetEvaluation(id) is not null);
        if (hasEvaluations)
            throw new DomainException("Нельзя удалить критерий: по нему уже выставлены оценки.");

        rubric.Remove(id);
        rubricRepository.Save(rubric);
    }
}
