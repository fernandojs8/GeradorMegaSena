using Gerador.Mega.Sena.Domain.Entities;
using Gerador.Mega.Sena.Infrastructure.Catalog;

namespace Gerador.Mega.Sena.Tests.Infrastructure;

public sealed class LotteryGameCatalogTests
{
    [Fact]
    public void GetAll_ReturnsAllSevenGames()
    {
        var catalog = new LotteryGameCatalog();

        IReadOnlyList<LotteryGame> games = catalog.GetAll();

        Assert.Equal(7, games.Count);
    }

    [Theory]
    [InlineData("mega-sena")]
    [InlineData("lotofacil")]
    [InlineData("quina")]
    [InlineData("lotomania")]
    [InlineData("dupla-sena")]
    [InlineData("timemania")]
    [InlineData("dia-de-sorte")]
    public void GetById_WithKnownId_ReturnsGame(string id)
    {
        var catalog = new LotteryGameCatalog();

        LotteryGame? game = catalog.GetById(id);

        Assert.NotNull(game);
        Assert.Equal(id, game.Id);
    }

    [Fact]
    public void GetById_WithUnknownId_ReturnsNull()
    {
        var catalog = new LotteryGameCatalog();

        LotteryGame? game = catalog.GetById("inexistente");

        Assert.Null(game);
    }

    [Fact]
    public void Timemania_HasSpecialPickWithTeams()
    {
        var catalog = new LotteryGameCatalog();

        LotteryGame? game = catalog.GetById("timemania");

        Assert.NotNull(game);
        Assert.True(game.HasSpecialPick);
        Assert.NotNull(game.SpecialPickLabel);
        Assert.NotNull(game.SpecialPickOptions);
        Assert.True(game.SpecialPickOptions.Count >= 10);
    }

    [Fact]
    public void DiaDeSorte_HasSpecialPickWithTwelveMonths()
    {
        var catalog = new LotteryGameCatalog();

        LotteryGame? game = catalog.GetById("dia-de-sorte");

        Assert.NotNull(game);
        Assert.True(game.HasSpecialPick);
        Assert.Equal(12, game.SpecialPickOptions!.Count);
    }

    [Theory]
    [InlineData("mega-sena")]
    [InlineData("lotofacil")]
    [InlineData("quina")]
    [InlineData("lotomania")]
    [InlineData("dupla-sena")]
    public void GamesWithoutSpecialPick_HasSpecialPickIsFalse(string id)
    {
        var catalog = new LotteryGameCatalog();

        LotteryGame? game = catalog.GetById(id);

        Assert.NotNull(game);
        Assert.False(game.HasSpecialPick);
    }

    [Fact]
    public void AllGames_HaveValidMinMaxPickRange()
    {
        var catalog = new LotteryGameCatalog();

        foreach (LotteryGame game in catalog.GetAll())
        {
            Assert.True(game.MinPicks >= 1, $"{game.Id}: MinPicks must be >= 1");
            Assert.True(game.MaxPicks >= game.MinPicks, $"{game.Id}: MaxPicks must be >= MinPicks");
            Assert.True(game.MinNumber >= 1, $"{game.Id}: MinNumber must be >= 1");
            Assert.True(game.MaxNumber >= game.MinNumber, $"{game.Id}: MaxNumber must be >= MinNumber");
        }
    }
}
