using POS.Models;
using Xunit;

namespace POS.Tests;

public class GradeTests
{
    [Fact]
    public void Constructor_WithinRange_CreatesGrade()
    {
        Assert.Equal(75, new Grade(75).Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(100.01)]
    public void Constructor_OutOfRange_ThrowsException(double value)
    {
        Assert.Throws<DomainException>(() => new Grade((decimal)value));
    }

    [Fact]
    public void FromFraction_RoundsToTwoDecimals()
    {
        Assert.Equal(33.33m, Grade.FromFraction(1m / 3m).Value);
    }

    [Fact]
    public void Zero_HasZeroValue()
    {
        Assert.Equal(0, Grade.Zero.Value);
    }
}
