using POS.Models;

namespace POS.UseCases.Students;

public interface IStudentRepository
{
    IReadOnlyList<Student> GetAll();
    Student? GetById(Guid id);
    void Add(Student student);
    void Update(Student student);
    void Remove(Guid id);
}
