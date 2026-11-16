namespace POS.UseCases.Criteria;

public sealed class AddCriterionHandler(IRubricRepository repository)
{
    public CriterionDto Handle(AddCriterionRequest request)
    {
        var rubric = repository.Get();
        var criterion = rubric.AddCriterion(request.Name, request.MaxScore, request.Weight);
        repository.Save(rubric);

        return new CriterionDto(
            criterion.Id, criterion.Name, criterion.MaxScore, criterion.Weight.Value);
    }
}
