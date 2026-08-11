using Gerador.Mega.Sena.Domain.Services;

namespace Gerador.Mega.Sena.Tests.Domain;

public sealed class UniquePlayGeneratorTests
{
    [Fact]
    public void Generate_WithFeasibleRequest_GeneratesRequestedUniqueCount()
    {
        var generator = new UniquePlayGenerator();

        PlayBatchResult result = generator.Generate(minNumber: 1, maxNumber: 10, picksPerPlay: 3, playCount: 20);

        Assert.Equal(20, result.Plays.Count);
        Assert.Equal(20, result.Plays.Distinct().Count());
        Assert.Null(result.Warning);
    }

    [Fact]
    public void Generate_EachPlayContainsExpectedAmountAndRange()
    {
        var generator = new UniquePlayGenerator();

        PlayBatchResult result = generator.Generate(minNumber: 1, maxNumber: 15, picksPerPlay: 5, playCount: 8);

        foreach (string play in result.Plays)
        {
            int[] numbers = play.Split(" - ").Select(int.Parse).ToArray();

            Assert.Equal(5, numbers.Length);
            Assert.All(numbers, n => Assert.InRange(n, 1, 15));
            Assert.Equal(numbers.Length, numbers.Distinct().Count());
            Assert.True(numbers.SequenceEqual(numbers.OrderBy(x => x)));
        }
    }
}
