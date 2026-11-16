using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Evaluations;
using POS.UseCases.Reports;
using POS.UseCases.Students;
using Xunit;

namespace POS.Tests;

public class UseCaseTests
{
    private readonly InMemoryStudentRepository _students = new();
    private readonly InMemoryRubricRepository _rubrics = new();

    private (Student Student, Criterion Criterion) Seed()
    {
        var student = new Student(Guid.NewGuid(), "Иванов Иван");
        _students.Add(student);
        var rubric = _rubrics.Get();
        var criterion = rubric.AddCriterion("Практика", 10, 1);
        _rubrics.Save(rubric);
        return (student, criterion);
    }

    private SetEvaluationHandler CreateSetHandler() =>
        new(_students, _rubrics, new EvaluationFactory());

    [Fact]
    public void SetEvaluation_ValidRequest_StoresEvaluation()
    {
        var (student, criterion) = Seed();

        var dto = CreateSetHandler().Handle(new SetEvaluationRequest(student.Id, criterion.Id, 7, null));

        Assert.Equal(7, dto.Score);
        Assert.Single(_students.GetById(student.Id)!.Evaluations);
    }

    [Fact]
    public void SetEvaluation_ScoreAboveMaximum_IsRejectedAndNothingStored()
    {
        var (student, criterion) = Seed();

        Assert.Throws<DomainException>(() =>
            CreateSetHandler().Handle(new SetEvaluationRequest(student.Id, criterion.Id, 12, null)));

        Assert.Empty(_students.GetById(student.Id)!.Evaluations);
    }

    [Fact]
    public void SetEvaluation_CriterionNotInRubric_IsRejectedAndNothingStored()
    {
        var (student, _) = Seed();

        Assert.Throws<DomainException>(() =>
            CreateSetHandler().Handle(new SetEvaluationRequest(student.Id, Guid.NewGuid(), 5, null)));

        Assert.Empty(_students.GetById(student.Id)!.Evaluations);
    }

    [Fact]
    public void RemoveCriterion_WithExistingEvaluations_IsRejected()
    {
        var (student, criterion) = Seed();
        CreateSetHandler().Handle(new SetEvaluationRequest(student.Id, criterion.Id, 7, null));

        var handler = new RemoveCriterionHandler(_rubrics, _students);

        Assert.Throws<DomainException>(() => handler.Handle(criterion.Id));
        Assert.Single(_rubrics.Get().Criteria);
    }

    [Fact]
    public void RemoveCriterion_WithoutEvaluations_RemovesIt()
    {
        var (_, criterion) = Seed();

        new RemoveCriterionHandler(_rubrics, _students).Handle(criterion.Id);

        Assert.Empty(_rubrics.Get().Criteria);
    }

    [Fact]
    public void CalculateGrade_UsesSelectedStrategy()
    {
        var (student, criterion) = Seed();
        var rubric = _rubrics.Get();
        rubric.AddCriterion("Экзамен", 10, 1);
        _rubrics.Save(rubric);
        CreateSetHandler().Handle(new SetEvaluationRequest(student.Id, criterion.Id, 10, null));

        var resolver = new GradeStrategyResolver(
            new IGradeCalculationStrategy[] { new GradedCriteriaStrategy(), new MissingAsZeroStrategy() });
        var handler = new CalculateGradeHandler(_students, _rubrics, new GradeCalculationService(), resolver);

        Assert.Equal(100m, handler.Handle(student.Id, "graded").Grade);
        Assert.Equal(50m, handler.Handle(student.Id, "missing-zero").Grade);
    }

    [Fact]
    public void GradeStrategyResolver_UnknownKey_ThrowsException()
    {
        var resolver = new GradeStrategyResolver(new IGradeCalculationStrategy[] { new GradedCriteriaStrategy() });
        Assert.Throws<DomainException>(() => resolver.Resolve("unknown"));
    }

    [Fact]
    public void GradeStrategyResolver_ListsAllRegisteredModes()
    {
        var resolver = new GradeStrategyResolver(
            new IGradeCalculationStrategy[] { new GradedCriteriaStrategy(), new MissingAsZeroStrategy() });
        Assert.Equal(2, resolver.GetModes().Count);
    }
}
