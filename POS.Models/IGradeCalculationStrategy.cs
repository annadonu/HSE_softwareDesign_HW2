namespace POS.Models;

public interface IGradeCalculationStrategy
{
    string Key { get; }
    string Description { get; }
    IReadOnlyList<CriterionScore> Collect(Student student, IReadOnlyCollection<Criterion> criteria);
}
