namespace POS.Models;

public sealed class Rubric
{
    private readonly List<Criterion> _criteria = new();

    public IReadOnlyList<Criterion> Criteria => _criteria.AsReadOnly();

    public Criterion AddCriterion(string name, decimal maxScore, decimal weight)
    {
        var criterion = new Criterion(Guid.NewGuid(), name, maxScore, new Weight(weight));
        Add(criterion);
        return criterion;
    }

    public void Add(Criterion criterion)
    {
        ArgumentNullException.ThrowIfNull(criterion);
        if (_criteria.Any(c => c.Id == criterion.Id))
            throw new DomainException("Критерий уже есть в рубрикаторе.");
        if (_criteria.Any(c => string.Equals(c.Name, criterion.Name, StringComparison.OrdinalIgnoreCase)))
            throw new DomainException("Критерий с таким названием уже есть в рубрикаторе.");
        _criteria.Add(criterion);
    }

    public void Remove(Guid criterionId)
    {
        var criterion = Find(criterionId)
            ?? throw new DomainException("Критерий не найден в рубрикаторе.");
        _criteria.Remove(criterion);
    }

    public Criterion? Find(Guid criterionId) =>
        _criteria.FirstOrDefault(c => c.Id == criterionId);

    public Criterion GetRequired(Guid criterionId) =>
        Find(criterionId) ?? throw new DomainException("Критерий не найден в рубрикаторе.");

    public static Rubric Restore(IEnumerable<Criterion> criteria)
    {
        var rubric = new Rubric();
        foreach (var criterion in criteria)
            rubric.Add(criterion);
        return rubric;
    }
}
