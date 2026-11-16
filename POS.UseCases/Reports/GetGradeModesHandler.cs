namespace POS.UseCases.Reports;

public sealed class GetGradeModesHandler(GradeStrategyResolver resolver)
{
    public IReadOnlyList<GradeModeDto> Handle() => resolver.GetModes();
}
