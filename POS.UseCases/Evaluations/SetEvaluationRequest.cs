namespace POS.UseCases.Evaluations;

public sealed record SetEvaluationRequest(
    Guid StudentId, Guid CriterionId, decimal Score, string? Comment);
