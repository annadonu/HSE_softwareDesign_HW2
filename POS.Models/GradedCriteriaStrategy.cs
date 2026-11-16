namespace POS.Models;

public sealed class GradedCriteriaStrategy : IGradeCalculationStrategy
{
    public string Key => "graded";

    public string Description => "Учитывать только выставленные оценки";

    public IReadOnlyList<CriterionScore> Collect(Student student, IReadOnlyCollection<Criterion> criteria)
    {
        var result = new List<CriterionScore>();
        foreach (var criterion in criteria)
        {
            var evaluation = student.GetEvaluation(criterion.Id);
            if (evaluation is not null)
                result.Add(new CriterionScore(criterion, evaluation.Score.Value));
        }
        return result;
    }
}
