namespace Gerador.Mega.Sena.Domain.Entities;

/// <summary>
/// Represents the official numeric rules for a lottery game.
/// </summary>
internal sealed class LotteryGame
{
    public LotteryGame(
        string id,
        string name,
        int minNumber,
        int maxNumber,
        int minPicks,
        int maxPicks,
        string description,
        string? specialPickLabel = null,
        IReadOnlyList<string>? specialPickOptions = null)
    {
        Id = id;
        Name = name;
        MinNumber = minNumber;
        MaxNumber = maxNumber;
        MinPicks = minPicks;
        MaxPicks = maxPicks;
        Description = description;
        SpecialPickLabel = specialPickLabel;
        SpecialPickOptions = specialPickOptions;
    }

    public string Id { get; }

    public string Name { get; }

    public int MinNumber { get; }

    public int MaxNumber { get; }

    public int MinPicks { get; }

    public int MaxPicks { get; }

    public string Description { get; }

    public string? SpecialPickLabel { get; }

    public IReadOnlyList<string>? SpecialPickOptions { get; }

    public bool HasFixedPickCount => MinPicks == MaxPicks;

    public bool HasSpecialPick => SpecialPickOptions is { Count: > 0 };
}
