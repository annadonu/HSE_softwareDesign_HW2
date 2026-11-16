namespace POS.Models;

public sealed class EvaluationFactory(TimeProvider? timeProvider = null)
{
    private readonly TimeProvider _timeProvider = timeProvider ?? TimeProvider.System;

    public Evaluation Create(Rubric rubric, Guid criterionId, decimal score, string? comment = null)
    {
        ArgumentNullException.ThrowIfNull(rubric);
        var criterion = rubric.GetRequired(criterionId);
        return new Evaluation(
            criterion.Id,
            new Score(score),
            criterion.MaxScore,
            _timeProvider.GetLocalNow(),
            comment);
    }
}
