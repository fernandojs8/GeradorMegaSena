using Gerador.Mega.Sena.Application.Abstractions;
using Gerador.Mega.Sena.Application.UseCases;
using Gerador.Mega.Sena.Presentation.Views;

namespace Gerador.Mega.Sena.Presentation.Controllers;

/// <summary>
/// Handles view events and orchestrates use case execution.
/// </summary>
internal sealed class MainController
{
    private readonly IMainView _view;
    private readonly ILotteryGameCatalog _catalog;
    private readonly ILocalizationService _localization;
    private readonly GeneratePlaysUseCase _generatePlaysUseCase;
    private IReadOnlyList<GameOptionViewModel> _games = [];

    public MainController(
        IMainView view,
        ILotteryGameCatalog catalog,
        ILocalizationService localization,
        GeneratePlaysUseCase generatePlaysUseCase)
    {
        _view = view;
        _catalog = catalog;
        _localization = localization;
        _generatePlaysUseCase = generatePlaysUseCase;

        _view.GenerateRequested += OnGenerateRequested;
        _view.LanguageChanged += OnLanguageChanged;
    }

    public void Initialize()
    {
        IReadOnlyList<LanguageOptionViewModel> languages = _localization
            .GetLanguages()
            .Select(lang => new LanguageOptionViewModel
            {
                Code = lang.Code,
                DisplayName = lang.DisplayName
            })
            .ToList();

        _view.BindLanguages(languages, _localization.DefaultLanguageCode);
        _localization.SetLanguage(_view.SelectedLanguageCode);

        ApplyLanguageDependentUi(selectedGameId: null);
    }

    public void ApplySelectedGameRules(string selectedGameId)
    {
        var game = _games.FirstOrDefault(x => x.Id == selectedGameId);
        if (game is null)
        {
            return;
        }

        ApplyRules(game);

        if (game.HasFixedPickCount)
        {
            _view.ShowInfo(string.Format(_localization.Get("status.fixedTemplate"), game.Name, game.MinPicks));
        }
        else
        {
            _view.ShowInfo(_localization.Get("status.ready"));
        }
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        _localization.SetLanguage(_view.SelectedLanguageCode);
        ApplyLanguageDependentUi(_view.SelectedGameId);
    }

    private void ApplyLanguageDependentUi(string? selectedGameId)
    {
        ApplyTexts();

        _games = _catalog
            .GetAll()
            .Select(game => new GameOptionViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = BuildLocalizedDescription(game.MinPicks, game.MaxPicks, game.MinNumber, game.MaxNumber),
                MinNumber = game.MinNumber,
                MaxNumber = game.MaxNumber,
                MinPicks = game.MinPicks,
                MaxPicks = game.MaxPicks
            })
            .ToList();

        _view.BindGames(_games, selectedGameId);

        var first = _games.FirstOrDefault();
        if (first is null)
        {
            _view.ShowError(_localization.Get("msg.noGames"));
            _view.SetGenerateEnabled(false);
            return;
        }

        GameOptionViewModel selected = _games.FirstOrDefault(x => x.Id == selectedGameId) ?? first;
        ApplyRules(selected);
        if (selected.HasFixedPickCount)
        {
            _view.ShowInfo(string.Format(_localization.Get("status.fixedTemplate"), selected.Name, selected.MinPicks));
        }
        else
        {
            _view.ShowInfo(_localization.Get("status.ready"));
        }
    }

    private void OnGenerateRequested(object? sender, EventArgs e)
    {
        _view.SetGenerateEnabled(false);

        try
        {
            var result = _generatePlaysUseCase.Execute(new GeneratePlaysRequest
            {
                GameId = _view.SelectedGameId,
                PicksPerPlay = _view.PicksPerPlay,
                PlayCount = _view.PlayCount
            });

            if (!result.IsSuccess)
            {
                _view.ShowError(result.Error ?? _localization.Get("msg.errorGeneric"));
                return;
            }

            _view.ShowSuccess(new GenerateOutputViewModel
            {
                GameName = result.GameName ?? "Jogo",
                PicksPerPlay = result.PicksPerPlay,
                Plays = result.Plays,
                Warning = result.Warning
            });
        }
        finally
        {
            _view.SetGenerateEnabled(true);
        }
    }

    private void ApplyRules(GameOptionViewModel game)
    {
        _view.ApplyGameRules(new GameRulesViewModel
        {
            Description = game.Description,
            MinPicks = game.MinPicks,
            MaxPicks = game.MaxPicks,
            HasFixedPickCount = game.HasFixedPickCount
        });
    }

    private void ApplyTexts()
    {
        _view.ApplyTexts(new UiTextViewModel
        {
            FormTitle = _localization.Get("form.title"),
            HeaderTitle = _localization.Get("header.title"),
            HeaderSubtitle = _localization.Get("header.subtitle"),
            LanguageLabel = _localization.Get("label.language"),
            GameLabel = _localization.Get("label.game"),
            PicksLabel = _localization.Get("label.picks"),
            PlayCountLabel = _localization.Get("label.playCount"),
            GenerateButton = _localization.Get("button.generate"),
            ResultsTitle = _localization.Get("results.title"),
            ReadyStatus = _localization.Get("status.ready"),
            FixedPickStatusTemplate = _localization.Get("status.fixedTemplate"),
            OutputGameLabel = _localization.Get("output.game"),
            OutputConfigLabel = _localization.Get("output.config"),
            WarningLabel = _localization.Get("output.warning"),
            SuccessStatusTemplate = _localization.Get("status.success"),
            SuccessStatusWithWarningTemplate = _localization.Get("status.successWarning"),
            DescriptionRangeTemplate = _localization.Get("desc.range"),
            DescriptionFixedTemplate = _localization.Get("desc.fixed")
        });
    }

    private string BuildLocalizedDescription(int minPicks, int maxPicks, int minNumber, int maxNumber)
    {
        if (minPicks == maxPicks)
        {
            return string.Format(_localization.Get("desc.fixed"), minPicks, minNumber, maxNumber);
        }

        return string.Format(_localization.Get("desc.range"), minPicks, maxPicks, minNumber, maxNumber);
    }
}
