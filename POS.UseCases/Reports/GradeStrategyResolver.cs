using POS.Models;

namespace POS.UseCases.Reports;

public sealed class GradeStrategyResolver(IEnumerable<IGradeCalculationStrategy> strategies)
{
    private readonly IReadOnlyList<IGradeCalculationStrategy> _strategies = strategies.ToList();

    public IReadOnlyList<GradeModeDto> GetModes() =>
        _strategies.Select(s => new GradeModeDto(s.Key, s.Description)).ToList();

    public IGradeCalculationStrategy Resolve(string key) =>
        _strategies.FirstOrDefault(s => s.Key == key)
            ?? throw new DomainException("Неизвестный режим расчёта итога.");
}
