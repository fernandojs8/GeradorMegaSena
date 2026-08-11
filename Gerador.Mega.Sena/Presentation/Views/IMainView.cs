namespace Gerador.Mega.Sena.Presentation.Views;

/// <summary>
/// Defines the UI contract used by the controller.
/// </summary>
internal interface IMainView
{
    event EventHandler GenerateRequested;

    event EventHandler LanguageChanged;

    string SelectedGameId { get; }

    string SelectedLanguageCode { get; }

    int PicksPerPlay { get; }

    int PlayCount { get; }

    void BindLanguages(IReadOnlyList<LanguageOptionViewModel> languages, string selectedLanguageCode);

    void BindGames(IReadOnlyList<GameOptionViewModel> games, string? selectedGameId);

    void ApplyGameRules(GameRulesViewModel rules);

    void ApplyTexts(UiTextViewModel texts);

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

    public required int MinNumber { get; init; }

    public required int MaxNumber { get; init; }

    public required int MinPicks { get; init; }

    public required int MaxPicks { get; init; }

    public bool HasFixedPickCount => MinPicks == MaxPicks;
}

/// <summary>
/// UI model for language option selection.
/// </summary>
internal sealed class LanguageOptionViewModel
{
    public required string Code { get; init; }

    public required string DisplayName { get; init; }
}

/// <summary>
/// Localized text bag for UI rendering.
/// </summary>
internal sealed class UiTextViewModel
{
    public required string FormTitle { get; init; }

    public required string HeaderTitle { get; init; }

    public required string HeaderSubtitle { get; init; }

    public required string LanguageLabel { get; init; }

    public required string GameLabel { get; init; }

    public required string PicksLabel { get; init; }

    public required string PlayCountLabel { get; init; }

    public required string GenerateButton { get; init; }

    public required string ResultsTitle { get; init; }

    public required string ReadyStatus { get; init; }

    public required string FixedPickStatusTemplate { get; init; }

    public required string OutputGameLabel { get; init; }

    public required string OutputConfigLabel { get; init; }

    public required string WarningLabel { get; init; }

    public required string SuccessStatusTemplate { get; init; }

    public required string SuccessStatusWithWarningTemplate { get; init; }

    public required string DescriptionRangeTemplate { get; init; }

    public required string DescriptionFixedTemplate { get; init; }
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
