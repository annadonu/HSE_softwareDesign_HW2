using POS.Models;

namespace POS.UseCases.Students;

public sealed class AddStudentHandler(IStudentRepository repository)
{
    public StudentDto Handle(AddStudentRequest request)
    {
        var student = new Student(Guid.NewGuid(), request.Name);
        repository.Add(student);
        return new StudentDto(student.Id, student.Name);
    }
}
