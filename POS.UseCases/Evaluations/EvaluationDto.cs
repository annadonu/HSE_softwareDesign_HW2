using POS.Models;

namespace POS.UseCases.Evaluations;

public sealed record EvaluationDto(
    Guid CriterionId,
    string CriterionName,
    decimal Score,
    decimal MaxScore,
    DateTimeOffset Date,
    string? Comment)
{
    public static EvaluationDto From(Criterion criterion, Evaluation evaluation) =>
        new(criterion.Id, criterion.Name, evaluation.Score.Value,
            criterion.MaxScore, evaluation.Date, evaluation.Comment);
}
