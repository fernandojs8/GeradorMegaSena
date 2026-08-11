using Gerador.Mega.Sena.Application.Abstractions;
using Gerador.Mega.Sena.Application.UseCases;
using Gerador.Mega.Sena.Domain.Entities;
using Gerador.Mega.Sena.Domain.Services;

namespace Gerador.Mega.Sena.Tests.Application;

public sealed class GeneratePlaysUseCaseTests
{
    [Fact]
    public void Execute_WithUnknownGame_ReturnsFailure()
    {
        var useCase = new GeneratePlaysUseCase(new FakeCatalog(), new UniquePlayGenerator());

        GeneratePlaysResult result = useCase.Execute(new GeneratePlaysRequest
        {
            GameId = "inexistente",
            PicksPerPlay = 6,
            PlayCount = 1
        });

        Assert.False(result.IsSuccess);
        Assert.Contains("nao encontrada", result.Error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Execute_WithPlayCountAboveSecurityLimit_ReturnsFailure()
    {
        var useCase = new GeneratePlaysUseCase(new FakeCatalog(), new UniquePlayGenerator());

        GeneratePlaysResult result = useCase.Execute(new GeneratePlaysRequest
        {
            GameId = "mini",
            PicksPerPlay = 2,
            PlayCount = 100001
        });

        Assert.False(result.IsSuccess);
        Assert.Contains("limite de seguranca", result.Error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Execute_WithCombinatorialOverflowRequest_ReturnsFailure()
    {
        var useCase = new GeneratePlaysUseCase(new FakeCatalog(), new UniquePlayGenerator());

        GeneratePlaysResult result = useCase.Execute(new GeneratePlaysRequest
        {
            GameId = "mini",
            PicksPerPlay = 4,
            PlayCount = 10
        });

        Assert.False(result.IsSuccess);
        Assert.Contains("Maximo possivel", result.Error, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Execute_WithValidRequest_ReturnsSuccessAndRequestedPlayCount()
    {
        var useCase = new GeneratePlaysUseCase(new FakeCatalog(), new UniquePlayGenerator());

        GeneratePlaysResult result = useCase.Execute(new GeneratePlaysRequest
        {
            GameId = "mini",
            PicksPerPlay = 3,
            PlayCount = 5
        });

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Plays.Count);
        Assert.Equal("Mini Loto", result.GameName);
    }

    [Fact]
    public void Execute_WithSpecialPickGame_AppendsPickToEachPlay()
    {
        var useCase = new GeneratePlaysUseCase(new FakeCatalogWithSpecialPick(), new UniquePlayGenerator());

        GeneratePlaysResult result = useCase.Execute(new GeneratePlaysRequest
        {
            GameId = "especial",
            PicksPerPlay = 2,
            PlayCount = 3
        });

        Assert.True(result.IsSuccess);
        Assert.All(result.Plays, play => Assert.Contains(" | ", play));
    }

    [Fact]
    public void Execute_WithExplicitSpecialPick_UsesThatPickForAllPlays()
    {
        var useCase = new GeneratePlaysUseCase(new FakeCatalogWithSpecialPick(), new UniquePlayGenerator());

        GeneratePlaysResult result = useCase.Execute(new GeneratePlaysRequest
        {
            GameId = "especial",
            PicksPerPlay = 2,
            PlayCount = 3,
            SpecialPick = "OpcaoA"
        });

        Assert.True(result.IsSuccess);
        Assert.All(result.Plays, play => Assert.EndsWith(" | OpcaoA", play));
    }

    private sealed class FakeCatalog : ILotteryGameCatalog
    {
        private static readonly IReadOnlyList<LotteryGame> Games =
        [
            new LotteryGame("mini", "Mini Loto", 1, 5, 2, 4, "Jogo de teste")
        ];

        public IReadOnlyList<LotteryGame> GetAll() => Games;

        public LotteryGame? GetById(string id) => Games.FirstOrDefault(x => x.Id == id);
    }

    private sealed class FakeCatalogWithSpecialPick : ILotteryGameCatalog
    {
        private static readonly IReadOnlyList<LotteryGame> Games =
        [
            new LotteryGame("especial", "Especial", 1, 10, 2, 4, "Jogo com especial",
                "Elemento", ["OpcaoA", "OpcaoB", "OpcaoC"])
        ];

        public IReadOnlyList<LotteryGame> GetAll() => Games;

        public LotteryGame? GetById(string id) => Games.FirstOrDefault(x => x.Id == id);
    }
}
