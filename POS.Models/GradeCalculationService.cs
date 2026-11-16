namespace POS.Models;

public sealed class GradeCalculationService
{
    public Grade Calculate(
        Student student,
        IReadOnlyCollection<Criterion> criteria,
        IGradeCalculationStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(strategy);

        var items = strategy.Collect(student, criteria);
        if (items.Count == 0)
            return Grade.Zero;

        var totalWeight = items.Sum(i => i.Criterion.Weight.Value);
        var weighted = items.Sum(i => i.Score / i.Criterion.MaxScore * i.Criterion.Weight.Value);
        return Grade.FromFraction(weighted / totalWeight);
    }
}
