namespace POS.UseCases.Students;

public sealed class GetStudentsHandler(IStudentRepository repository)
{
    public IReadOnlyList<StudentDto> Handle() =>
        repository.GetAll().Select(s => new StudentDto(s.Id, s.Name)).ToList();
}
