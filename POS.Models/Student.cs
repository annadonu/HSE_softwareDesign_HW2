namespace POS.Models;

public sealed class Student
{
    private readonly List<Evaluation> _evaluations = new();

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyList<Evaluation> Evaluations => _evaluations.AsReadOnly();

    public Student(Guid id, string name)
    {
        if (id == Guid.Empty)
            throw new DomainException("Идентификатор студента не может быть пустым.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Имя студента не может быть пустым.");

        Id = id;
        Name = name.Trim();
    }

    public void AddEvaluation(Evaluation evaluation)
    {
        ArgumentNullException.ThrowIfNull(evaluation);
        if (_evaluations.Any(e => e.CriterionId == evaluation.CriterionId))
            throw new DomainException("Оценка по этому критерию уже выставлена.");
        _evaluations.Add(evaluation);
    }

    public void UpdateEvaluation(Evaluation evaluation)
    {
        ArgumentNullException.ThrowIfNull(evaluation);
        var index = _evaluations.FindIndex(e => e.CriterionId == evaluation.CriterionId);
        if (index < 0)
            throw new DomainException("Оценка по этому критерию ещё не выставлена.");
        _evaluations[index] = evaluation;
    }

    public void RemoveEvaluation(Guid criterionId)
    {
        var evaluation = _evaluations.FirstOrDefault(e => e.CriterionId == criterionId)
            ?? throw new DomainException("Оценка по этому критерию ещё не выставлена.");
        _evaluations.Remove(evaluation);
    }

    public Evaluation? GetEvaluation(Guid criterionId) =>
        _evaluations.FirstOrDefault(e => e.CriterionId == criterionId);

    public static Student Restore(Guid id, string name, IEnumerable<Evaluation> evaluations)
    {
        var student = new Student(id, name);
        foreach (var evaluation in evaluations)
            student.AddEvaluation(evaluation);
        return student;
    }
}
