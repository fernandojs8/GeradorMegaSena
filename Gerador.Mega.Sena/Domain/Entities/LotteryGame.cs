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
        string description)
    {
        Id = id;
        Name = name;
        MinNumber = minNumber;
        MaxNumber = maxNumber;
        MinPicks = minPicks;
        MaxPicks = maxPicks;
        Description = description;
    }

    public string Id { get; }

    public string Name { get; }

    public int MinNumber { get; }

    public int MaxNumber { get; }

    public int MinPicks { get; }

    public int MaxPicks { get; }

    public string Description { get; }

    public bool HasFixedPickCount => MinPicks == MaxPicks;
}
