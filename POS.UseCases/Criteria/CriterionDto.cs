namespace POS.UseCases.Criteria;

public sealed record CriterionDto(Guid Id, string Name, decimal MaxScore, decimal Weight);
