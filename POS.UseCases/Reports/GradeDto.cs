namespace POS.UseCases.Reports;

public sealed record GradeDto(Guid StudentId, string StudentName, decimal Grade);
