using POS.Models;
using Xunit;

namespace POS.Tests;

public class WeightTests
{
    [Fact]
    public void Constructor_WithPositiveValue_CreatesWeight()
    {
        var weight = new Weight(2);
        Assert.Equal(2, weight.Value);
    }

    [Fact]
    public void Constructor_WithZero_ThrowsException()
    {
        Assert.Throws<DomainException>(() => new Weight(0));
    }

    [Fact]
    public void Constructor_WithNegativeValue_ThrowsException()
    {
        Assert.Throws<DomainException>(() => new Weight(-1));
    }
}
