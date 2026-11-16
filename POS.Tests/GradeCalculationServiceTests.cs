using POS.Models;
using Xunit;

namespace POS.Tests;

public class GradeCalculationServiceTests
{
    private readonly GradeCalculationService _service = new();
    private readonly EvaluationFactory _factory = new();
    private readonly Rubric _rubric = new();

    [Fact]
    public void Calculate_WithAllEvaluations_ReturnsWeightedGrade()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        var first = _rubric.AddCriterion("Практика", 10, 2);
        var second = _rubric.AddCriterion("Экзамен", 20, 1);

        student.AddEvaluation(_factory.Create(_rubric, first.Id, 5));
        student.AddEvaluation(_factory.Create(_rubric, second.Id, 10));

        var result = _service.Calculate(student, _rubric.Criteria, new GradedCriteriaStrategy());
        Assert.Equal(50m, result.Value);
    }

    [Fact]
    public void Calculate_GradedCriteriaStrategy_IgnoresMissingGrades()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        var first = _rubric.AddCriterion("Практика", 10, 1);
        _rubric.AddCriterion("Экзамен", 10, 1);
        student.AddEvaluation(_factory.Create(_rubric, first.Id, 10));

        var result = _service.Calculate(student, _rubric.Criteria, new GradedCriteriaStrategy());
        Assert.Equal(100m, result.Value);
    }

    [Fact]
    public void Calculate_MissingAsZeroStrategy_ConsidersMissingGrades()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        var first = _rubric.AddCriterion("Практика", 10, 1);
        _rubric.AddCriterion("Экзамен", 10, 1);
        student.AddEvaluation(_factory.Create(_rubric, first.Id, 10));

        var result = _service.Calculate(student, _rubric.Criteria, new MissingAsZeroStrategy());
        Assert.Equal(50m, result.Value);
    }

    [Fact]
    public void Calculate_RoundsResultToTwoDecimals()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        var criterion = _rubric.AddCriterion("Практика", 3, 1);
        student.AddEvaluation(_factory.Create(_rubric, criterion.Id, 1));

        var result = _service.Calculate(student, _rubric.Criteria, new GradedCriteriaStrategy());
        Assert.Equal(33.33m, result.Value);
    }

    [Fact]
    public void Calculate_ChangesWhenEvaluationIsUpdated()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        var criterion = _rubric.AddCriterion("Практика", 10, 1);
        student.AddEvaluation(_factory.Create(_rubric, criterion.Id, 4));
        var before = _service.Calculate(student, _rubric.Criteria, new GradedCriteriaStrategy());

        student.UpdateEvaluation(_factory.Create(_rubric, criterion.Id, 8));
        var after = _service.Calculate(student, _rubric.Criteria, new GradedCriteriaStrategy());

        Assert.Equal(40m, before.Value);
        Assert.Equal(80m, after.Value);
    }

    [Fact]
    public void Calculate_WithoutEvaluations_ReturnsZero()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        _rubric.AddCriterion("Практика", 10, 1);

        var result = _service.Calculate(student, _rubric.Criteria, new GradedCriteriaStrategy());
        Assert.Equal(0m, result.Value);
    }

    [Fact]
    public void Calculate_WithoutCriteria_ReturnsZero()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        var result = _service.Calculate(student, Array.Empty<Criterion>(), new MissingAsZeroStrategy());
        Assert.Equal(0m, result.Value);
    }
}
