using Gerador.Mega.Sena.Application.Abstractions;
using Gerador.Mega.Sena.Domain.Services;

namespace Gerador.Mega.Sena.Application.UseCases;

/// <summary>
/// Coordinates validation and generation of lottery plays.
/// </summary>
internal sealed class GeneratePlaysUseCase
{
    private const int MaxAllowedPlayCount = 100000;

    private readonly ILotteryGameCatalog _catalog;
    private readonly UniquePlayGenerator _generator;

    public GeneratePlaysUseCase(ILotteryGameCatalog catalog, UniquePlayGenerator generator)
    {
        _catalog = catalog;
        _generator = generator;
    }

    public GeneratePlaysResult Execute(GeneratePlaysRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.GameId))
        {
            return GeneratePlaysResult.Fail("Modalidade invalida.");
        }

        if (request.PlayCount <= 0)
        {
            return GeneratePlaysResult.Fail("Quantidade de jogadas deve ser maior que zero.");
        }

        if (request.PlayCount > MaxAllowedPlayCount)
        {
            return GeneratePlaysResult.Fail($"Quantidade de jogadas excede o limite de seguranca de {MaxAllowedPlayCount}.");
        }

        var game = _catalog.GetById(request.GameId.Trim());
        if (game is null)
        {
            return GeneratePlaysResult.Fail("Modalidade nao encontrada.");
        }

        if (request.PicksPerPlay < game.MinPicks || request.PicksPerPlay > game.MaxPicks)
        {
            return GeneratePlaysResult.Fail(
                $"Quantidade invalida para {game.Name}. Use de {game.MinPicks} a {game.MaxPicks} numeros.");
        }

        int rangeSize = (game.MaxNumber - game.MinNumber) + 1;
        if (request.PicksPerPlay > rangeSize)
        {
            return GeneratePlaysResult.Fail("Nao e possivel gerar numeros unicos nessa quantidade para o intervalo informado.");
        }

        long maxPossible = CombinationMath.BoundedCombination(rangeSize, request.PicksPerPlay, request.PlayCount);
        if (maxPossible < request.PlayCount)
        {
            return GeneratePlaysResult.Fail(
                $"Nao e possivel gerar {request.PlayCount} jogadas unicas. Maximo possivel: {maxPossible}.");
        }

        PlayBatchResult batch = _generator.Generate(game.MinNumber, game.MaxNumber, request.PicksPerPlay, request.PlayCount);

        IReadOnlyList<string> finalPlays = batch.Plays;
        if (game.HasSpecialPick)
        {
            var rng = new Random();
            finalPlays = batch.Plays
                .Select(play =>
                {
                    string pick = string.IsNullOrEmpty(request.SpecialPick)
                        ? game.SpecialPickOptions![rng.Next(game.SpecialPickOptions.Count)]
                        : request.SpecialPick;
                    return $"{play} | {pick}";
                })
                .ToList();
        }

        return GeneratePlaysResult.Success(game.Name, request.PicksPerPlay, finalPlays, batch.Warning);
    }
}

/// <summary>
/// Input model for play generation.
/// </summary>
internal sealed class GeneratePlaysRequest
{
    public required string GameId { get; init; }

    public required int PicksPerPlay { get; init; }

    public required int PlayCount { get; init; }

    /// <summary>
    /// Specific special pick value; null means a random option is chosen per play.
    /// </summary>
    public string? SpecialPick { get; init; }
}

/// <summary>
/// Output model for play generation.
/// </summary>
internal sealed class GeneratePlaysResult
{
    private GeneratePlaysResult()
    {
    }

    public bool IsSuccess { get; private set; }

    public string? Error { get; private set; }

    public string? Warning { get; private set; }

    public string? GameName { get; private set; }

    public int PicksPerPlay { get; private set; }

    public IReadOnlyList<string> Plays { get; private set; } = [];

    public static GeneratePlaysResult Fail(string error)
    {
        return new GeneratePlaysResult
        {
            IsSuccess = false,
            Error = error
        };
    }

    public static GeneratePlaysResult Success(string gameName, int picksPerPlay, IReadOnlyList<string> plays, string? warning)
    {
        return new GeneratePlaysResult
        {
            IsSuccess = true,
            GameName = gameName,
            PicksPerPlay = picksPerPlay,
            Plays = plays,
            Warning = warning
        };
    }
}
