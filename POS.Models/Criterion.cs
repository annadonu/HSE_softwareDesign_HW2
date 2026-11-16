namespace POS.Models;

public sealed class Criterion
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal MaxScore { get; }
    public Weight Weight { get; }

    public Criterion(Guid id, string name, decimal maxScore, Weight weight)
    {
        if (id == Guid.Empty)
            throw new DomainException("Идентификатор критерия не может быть пустым.");
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Название критерия не может быть пустым.");
        if (maxScore <= 0)
            throw new DomainException("Максимальный балл критерия должен быть положительным.");

        Id = id;
        Name = name.Trim();
        MaxScore = maxScore;
        Weight = weight ?? throw new ArgumentNullException(nameof(weight));
    }
}
