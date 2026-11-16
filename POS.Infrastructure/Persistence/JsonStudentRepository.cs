using POS.Models;
using POS.UseCases.Students;

namespace POS.Infrastructure.Persistence;

internal sealed class JsonStudentRepository(JsonStorage storage) : IStudentRepository
{
    public IReadOnlyList<Student> GetAll() =>
        storage.Load().Students.Select(ToDomain).ToList();

    public Student? GetById(Guid id)
    {
        var dto = storage.Load().Students.FirstOrDefault(s => s.Id == id);
        return dto is null ? null : ToDomain(dto);
    }

    public void Add(Student student)
    {
        var data = storage.Load();
        if (data.Students.Any(s => s.Id == student.Id))
            throw new DomainException("Студент уже существует.");
        data.Students.Add(ToDto(student));
        storage.Save(data);
    }

    public void Update(Student student)
    {
        var data = storage.Load();
        var index = data.Students.FindIndex(s => s.Id == student.Id);
        if (index < 0)
            throw new DomainException("Студент не найден.");
        data.Students[index] = ToDto(student);
        storage.Save(data);
    }

    public void Remove(Guid id)
    {
        var data = storage.Load();
        var student = data.Students.FirstOrDefault(s => s.Id == id);
        if (student is null)
            throw new DomainException("Студент не найден.");
        data.Students.Remove(student);
        storage.Save(data);
    }

    private static Student ToDomain(StudentStorageDto dto)
    {
        var evaluations = dto.Evaluations.Select(e =>
            Evaluation.Restore(e.CriterionId, new Score(e.Score), e.MaxScore, e.Date, e.Comment));
        return Student.Restore(dto.Id, dto.Name, evaluations);
    }

    private static StudentStorageDto ToDto(Student student) =>
        new(student.Id, student.Name,
            student.Evaluations.Select(e =>
                new EvaluationStorageDto(
                    e.CriterionId, e.Score.Value, e.MaxScore, e.Date, e.Comment))
                .ToList());
}
