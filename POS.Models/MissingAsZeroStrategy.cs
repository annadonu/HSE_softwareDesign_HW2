namespace POS.Models;

public sealed class MissingAsZeroStrategy : IGradeCalculationStrategy
{
    public string Key => "missing-zero";

    public string Description => "Невыставленные оценки считать нулевыми";

    public IReadOnlyList<CriterionScore> Collect(Student student, IReadOnlyCollection<Criterion> criteria) =>
        criteria
            .Select(c => new CriterionScore(c, student.GetEvaluation(c.Id)?.Score.Value ?? 0))
            .ToList();
}
