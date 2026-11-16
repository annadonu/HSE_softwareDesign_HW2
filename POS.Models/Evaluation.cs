namespace POS.Models;

public sealed record Evaluation
{
    public Guid CriterionId { get; }
    public Score Score { get; }
    public decimal MaxScore { get; }
    public DateTimeOffset Date { get; }
    public string? Comment { get; }

    internal Evaluation(
        Guid criterionId,
        Score score,
        decimal maxScore,
        DateTimeOffset date,
        string? comment)
    {
        if (criterionId == Guid.Empty)
            throw new DomainException("Идентификатор критерия не может быть пустым.");
        if (maxScore <= 0)
            throw new DomainException("Максимальный балл должен быть положительным.");

        Score = score ?? throw new ArgumentNullException(nameof(score));
        if (score.Value > maxScore)
            throw new DomainException($"Балл {score.Value} превышает максимум {maxScore}.");

        CriterionId = criterionId;
        MaxScore = maxScore;
        Date = date;
        Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
    }

    public static Evaluation Restore(
        Guid criterionId,
        Score score,
        decimal maxScore,
        DateTimeOffset date,
        string? comment) =>
        new(criterionId, score, maxScore, date, comment);
}
