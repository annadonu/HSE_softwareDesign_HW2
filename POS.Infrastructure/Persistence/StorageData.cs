namespace POS.Infrastructure.Persistence;

public sealed class StorageData
{
    public List<StudentStorageDto> Students { get; set; } = new();
    public List<CriterionStorageDto> Criteria { get; set; } = new();
}

public sealed record StudentStorageDto(
    Guid Id, string Name, List<EvaluationStorageDto> Evaluations);

public sealed record EvaluationStorageDto(
    Guid CriterionId, decimal Score, decimal MaxScore,
    DateTimeOffset Date, string? Comment);

public sealed record CriterionStorageDto(
    Guid Id, string Name, decimal MaxScore, decimal Weight);
