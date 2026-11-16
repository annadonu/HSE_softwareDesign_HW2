using POS.Models;
using Xunit;

namespace POS.Tests;

public class RubricTests
{
    [Fact]
    public void AddCriterion_AddsCriterionToRubric()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Практика", 10, 2);
        Assert.Single(rubric.Criteria);
        Assert.Equal(criterion.Id, rubric.Criteria[0].Id);
    }

    [Theory]
    [InlineData("Практика")]
    [InlineData("  практика  ")]
    [InlineData("ПРАКТИКА")]
    public void AddCriterion_DuplicateName_ThrowsException(string name)
    {
        var rubric = new Rubric();
        rubric.AddCriterion("Практика", 10, 1);
        Assert.Throws<DomainException>(() => rubric.AddCriterion(name, 5, 1));
    }

    [Fact]
    public void AddCriterion_InvalidMaxScore_ThrowsException()
    {
        var rubric = new Rubric();
        Assert.Throws<DomainException>(() => rubric.AddCriterion("Практика", 0, 1));
    }

    [Fact]
    public void AddCriterion_InvalidWeight_ThrowsException()
    {
        var rubric = new Rubric();
        Assert.Throws<DomainException>(() => rubric.AddCriterion("Практика", 10, 0));
    }

    [Fact]
    public void Remove_RemovesCriterion()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Практика", 10, 1);
        rubric.Remove(criterion.Id);
        Assert.Empty(rubric.Criteria);
    }

    [Fact]
    public void Remove_UnknownCriterion_ThrowsException()
    {
        var rubric = new Rubric();
        Assert.Throws<DomainException>(() => rubric.Remove(Guid.NewGuid()));
    }

    [Fact]
    public void GetRequired_UnknownCriterion_ThrowsException()
    {
        var rubric = new Rubric();
        Assert.Throws<DomainException>(() => rubric.GetRequired(Guid.NewGuid()));
    }
}
