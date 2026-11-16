using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.Tests;

internal sealed class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();

    public IReadOnlyList<Student> GetAll() => _students.ToList();

    public Student? GetById(Guid id) => _students.FirstOrDefault(s => s.Id == id);

    public void Add(Student student) => _students.Add(student);

    public void Update(Student student)
    {
    }

    public void Remove(Guid id) => _students.RemoveAll(s => s.Id == id);
}

internal sealed class InMemoryRubricRepository : IRubricRepository
{
    private Rubric _rubric = new();

    public Rubric Get() => _rubric;

    public void Save(Rubric rubric) => _rubric = rubric;
}
