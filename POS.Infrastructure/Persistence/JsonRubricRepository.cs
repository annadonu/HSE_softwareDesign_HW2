using POS.Models;
using POS.UseCases.Criteria;

namespace POS.Infrastructure.Persistence;

internal sealed class JsonRubricRepository(JsonStorage storage) : IRubricRepository
{
    public Rubric Get() =>
        Rubric.Restore(storage.Load().Criteria.Select(ToDomain));

    public void Save(Rubric rubric)
    {
        var data = storage.Load();
        data.Criteria = rubric.Criteria.Select(ToDto).ToList();
        storage.Save(data);
    }

    private static Criterion ToDomain(CriterionStorageDto dto) =>
        new(dto.Id, dto.Name, dto.MaxScore, new Weight(dto.Weight));

    private static CriterionStorageDto ToDto(Criterion criterion) =>
        new(criterion.Id, criterion.Name, criterion.MaxScore, criterion.Weight.Value);
}
