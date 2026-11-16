namespace POS.UseCases.Criteria;

public sealed record AddCriterionRequest(string Name, decimal MaxScore, decimal Weight);
