using POS.Models;
using Xunit;

namespace POS.Tests;

public class ScoreTests
{
    [Fact]
    public void Constructor_WithPositiveValue_CreatesScore()
    {
        var score = new Score(8);
        Assert.Equal(8, score.Value);
    }

    [Fact]
    public void Constructor_WithZero_CreatesScore()
    {
        var score = new Score(0);
        Assert.Equal(0, score.Value);
    }

    [Fact]
    public void Constructor_WithNegativeValue_ThrowsException()
    {
        Assert.Throws<DomainException>(() => new Score(-1));
    }
}
