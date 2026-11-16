using POS.Models;
using Xunit;

namespace POS.Tests;

public class EvaluationFactoryTests
{
    private readonly EvaluationFactory _factory = new();
    private readonly Rubric _rubric = new();
    private readonly Criterion _criterion;

    public EvaluationFactoryTests()
    {
        _criterion = _rubric.AddCriterion("Контрольная", 10, 1);
    }

    [Fact]
    public void Create_WithValidScore_CreatesEvaluation()
    {
        var evaluation = _factory.Create(_rubric, _criterion.Id, 8);
        Assert.Equal(8, evaluation.Score.Value);
        Assert.Equal(_criterion.Id, evaluation.CriterionId);
        Assert.Equal(10, evaluation.MaxScore);
    }

    [Fact]
    public void Create_WithScoreEqualToMaximum_CreatesEvaluation()
    {
        var evaluation = _factory.Create(_rubric, _criterion.Id, 10);
        Assert.Equal(10, evaluation.Score.Value);
    }

    [Fact]
    public void Create_WithZeroScore_CreatesEvaluation()
    {
        var evaluation = _factory.Create(_rubric, _criterion.Id, 0);
        Assert.Equal(0, evaluation.Score.Value);
    }

    [Fact]
    public void Create_WithScoreAboveMaximum_ThrowsException()
    {
        Assert.Throws<DomainException>(() => _factory.Create(_rubric, _criterion.Id, 11));
    }

    [Fact]
    public void Create_WithNegativeScore_ThrowsException()
    {
        Assert.Throws<DomainException>(() => _factory.Create(_rubric, _criterion.Id, -1));
    }

    [Fact]
    public void Create_WithCriterionNotInRubric_ThrowsException()
    {
        Assert.Throws<DomainException>(() => _factory.Create(_rubric, Guid.NewGuid(), 5));
    }

    [Fact]
    public void Create_TrimsCommentAndDropsBlankOne()
    {
        var withComment = _factory.Create(_rubric, _criterion.Id, 5, "  хорошо  ");
        var blank = _factory.Create(_rubric, _criterion.Id, 5, "   ");
        Assert.Equal("хорошо", withComment.Comment);
        Assert.Null(blank.Comment);
    }
}
