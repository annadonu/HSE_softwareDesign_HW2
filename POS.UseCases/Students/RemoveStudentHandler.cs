using POS.Models;

namespace POS.UseCases.Students;

public sealed class RemoveStudentHandler(IStudentRepository repository)
{
    public void Handle(Guid id)
    {
        if (repository.GetById(id) is null)
            throw new DomainException("Студент не найден.");
        repository.Remove(id);
    }
}
