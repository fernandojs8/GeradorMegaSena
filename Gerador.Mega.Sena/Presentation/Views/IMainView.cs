namespace Gerador.Mega.Sena.Presentation.Views;

/// <summary>
/// Defines the UI contract used by the controller.
/// </summary>
internal interface IMainView
{
    event EventHandler GenerateRequested;

    string SelectedGameId { get; }

    int PicksPerPlay { get; }

    int PlayCount { get; }

    void BindGames(IReadOnlyList<GameOptionViewModel> games);

    void ApplyGameRules(GameRulesViewModel rules);

    void ShowError(string message);

    void ShowSuccess(GenerateOutputViewModel output);

    void ShowInfo(string message);

    void SetGenerateEnabled(bool enabled);
}

/// <summary>
/// UI model used to bind game options.
/// </summary>
internal sealed class GameOptionViewModel
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required int MinPicks { get; init; }

    public required int MaxPicks { get; init; }

    public bool HasFixedPickCount => MinPicks == MaxPicks;
}

/// <summary>
/// UI model with game numeric constraints.
/// </summary>
internal sealed class GameRulesViewModel
{
    public required string Description { get; init; }

    public required int MinPicks { get; init; }

    public required int MaxPicks { get; init; }

    public required bool HasFixedPickCount { get; init; }
}

/// <summary>
/// UI model for successful generation output.
/// </summary>
internal sealed class GenerateOutputViewModel
{
    public required string GameName { get; init; }

    public required int PicksPerPlay { get; init; }

    public required IReadOnlyList<string> Plays { get; init; }

    public string? Warning { get; init; }
}
