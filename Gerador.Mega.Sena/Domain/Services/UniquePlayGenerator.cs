namespace Gerador.Mega.Sena.Domain.Services;

/// <summary>
/// Generates unique plays for a numeric lottery game.
/// </summary>
internal sealed class UniquePlayGenerator
{
    private const int MaxAttemptsMultiplier = 200;

    public PlayBatchResult Generate(int minNumber, int maxNumber, int picksPerPlay, int playCount)
    {
        int rangeSize = (maxNumber - minNumber) + 1;
        int attempts = 0;
        int maxAttempts = Math.Max(playCount * MaxAttemptsMultiplier, 1000);

        var random = new Random();
        var plays = new List<string>(playCount);
        var playIndex = new HashSet<string>();

        while (plays.Count < playCount && attempts < maxAttempts)
        {
            var candidate = GenerateSinglePlay(random, minNumber, rangeSize, picksPerPlay)
                .OrderBy(x => x)
                .Select(x => x < 10 ? $"0{x}" : x.ToString());

            string play = string.Join(" - ", candidate);
            attempts++;

            if (playIndex.Add(play))
            {
                plays.Add(play);
            }
        }

        return new PlayBatchResult(
            plays,
            plays.Count < playCount
                ? "Nao foi possivel concluir todas as jogadas no tempo esperado. Tente reduzir a quantidade solicitada."
                : null);
    }

    private static IEnumerable<int> GenerateSinglePlay(Random random, int minNumber, int rangeSize, int picksPerPlay)
    {
        var pool = Enumerable.Range(minNumber, rangeSize).ToList();

        for (int i = 0; i < picksPerPlay; i++)
        {
            int drawnIndex = random.Next(i, pool.Count);
            int temp = pool[i];
            pool[i] = pool[drawnIndex];
            pool[drawnIndex] = temp;
        }

        return pool.Take(picksPerPlay);
    }
}

/// <summary>
/// Represents generated plays and optional warning.
/// </summary>
internal sealed class PlayBatchResult
{
    public PlayBatchResult(IReadOnlyList<string> plays, string? warning)
    {
        Plays = plays;
        Warning = warning;
    }

    public IReadOnlyList<string> Plays { get; }

    public string? Warning { get; }
}
