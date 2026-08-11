using Gerador.Mega.Sena.Domain.Services;

namespace Gerador.Mega.Sena.Tests.Domain;

public sealed class CombinationMathTests
{
    [Theory]
    [InlineData(-1, 5)]
    [InlineData(5, -1)]
    [InlineData(3, 5)]
    public void BoundedCombination_InvalidArguments_ReturnsZero(int n, int k)
    {
        long result = CombinationMath.BoundedCombination(n, k, 100);

        Assert.Equal(0, result);
    }

    [Fact]
    public void BoundedCombination_LimitReached_ReturnsLimit()
    {
        long result = CombinationMath.BoundedCombination(60, 6, 10);

        Assert.Equal(10, result);
    }

    [Fact]
    public void BoundedCombination_ExactSmallValue_ReturnsExpected()
    {
        long result = CombinationMath.BoundedCombination(5, 2, 1000);

        Assert.Equal(10, result);
    }
}
