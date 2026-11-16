namespace POS.UseCases.Criteria;

public sealed class GetCriteriaHandler(IRubricRepository repository)
{
    public IReadOnlyList<CriterionDto> Handle() =>
        repository.Get().Criteria
            .Select(c => new CriterionDto(c.Id, c.Name, c.MaxScore, c.Weight.Value))
            .ToList();
}
