using POS.Models;
using Xunit;

namespace POS.Tests;

public class StudentTests
{
    private static readonly Rubric Rubric = new();
    private static readonly Criterion Criterion = Rubric.AddCriterion("Практика", 10, 1);

    private static Evaluation CreateEvaluation(decimal score) =>
        new EvaluationFactory().Create(Rubric, Criterion.Id, score);

    [Fact]
    public void Constructor_WithEmptyName_ThrowsException()
    {
        Assert.Throws<DomainException>(() => new Student(Guid.NewGuid(), " "));
    }

    [Fact]
    public void Constructor_WithEmptyId_ThrowsException()
    {
        Assert.Throws<DomainException>(() => new Student(Guid.Empty, "Иванов Иван"));
    }

    [Fact]
    public void AddEvaluation_AddsEvaluation()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        student.AddEvaluation(CreateEvaluation(8));
        Assert.Single(student.Evaluations);
        Assert.Equal(8, student.Evaluations[0].Score.Value);
    }

    [Fact]
    public void AddEvaluation_DuplicateCriterion_ThrowsException()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        student.AddEvaluation(CreateEvaluation(8));
        Assert.Throws<DomainException>(() => student.AddEvaluation(CreateEvaluation(9)));
    }

    [Fact]
    public void UpdateEvaluation_ChangesScore()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        student.AddEvaluation(CreateEvaluation(5));
        student.UpdateEvaluation(CreateEvaluation(9));
        Assert.Single(student.Evaluations);
        Assert.Equal(9, student.Evaluations[0].Score.Value);
    }

    [Fact]
    public void UpdateEvaluation_WhenEvaluationDoesNotExist_ThrowsException()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        Assert.Throws<DomainException>(() => student.UpdateEvaluation(CreateEvaluation(9)));
    }

    [Fact]
    public void RemoveEvaluation_RemovesEvaluation()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        student.AddEvaluation(CreateEvaluation(8));
        student.RemoveEvaluation(Criterion.Id);
        Assert.Empty(student.Evaluations);
    }

    [Fact]
    public void RemoveEvaluation_WhenEvaluationDoesNotExist_ThrowsException()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        Assert.Throws<DomainException>(() => student.RemoveEvaluation(Criterion.Id));
    }
}
