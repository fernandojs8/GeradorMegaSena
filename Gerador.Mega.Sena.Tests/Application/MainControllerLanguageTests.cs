using Gerador.Mega.Sena.Application.Abstractions;
using Gerador.Mega.Sena.Application.UseCases;
using Gerador.Mega.Sena.Domain.Entities;
using Gerador.Mega.Sena.Domain.Services;
using Gerador.Mega.Sena.Infrastructure.Localization;
using Gerador.Mega.Sena.Presentation.Controllers;
using Gerador.Mega.Sena.Presentation.Views;

namespace Gerador.Mega.Sena.Tests.Application;

public sealed class MainControllerLanguageTests
{
    [Fact]
    public void Initialize_BindsLanguagesAndPortugueseTextsByDefault()
    {
        var view = new FakeView();
        var controller = CreateController(view);

        controller.Initialize();

        Assert.Equal("pt", view.SelectedLanguageCode);
        Assert.Equal("Gerar Jogadas", view.LastTexts?.GenerateButton);
    }

    [Fact]
    public void Initialize_SetsExportButtonText()
    {
        var view = new FakeView();
        var controller = CreateController(view);

        controller.Initialize();

        Assert.Equal("Exportar", view.LastTexts?.ExportButton);
    }

    [Fact]
    public void LanguageChanged_ToEnglish_UpdatesViewTexts()
    {
        var view = new FakeView();
        var controller = CreateController(view);

        controller.Initialize();
        view.ChangeLanguage("en");

        Assert.Equal("Generate Plays", view.LastTexts?.GenerateButton);
        Assert.Equal("🌐 Language", view.LastTexts?.LanguageLabel);
    }

    [Fact]
    public void LanguageChanged_ToEnglish_UpdatesExportButtonText()
    {
        var view = new FakeView();
        var controller = CreateController(view);

        controller.Initialize();
        view.ChangeLanguage("en");

        Assert.Equal("Export", view.LastTexts?.ExportButton);
    }

    private static MainController CreateController(FakeView view)
    {
        ILotteryGameCatalog catalog = new FakeCatalog();
        var localization = new InMemoryLocalizationService();
        var useCase = new GeneratePlaysUseCase(catalog, new UniquePlayGenerator());

        return new MainController(view, catalog, localization, useCase);
    }

    private sealed class FakeCatalog : ILotteryGameCatalog
    {
        private static readonly IReadOnlyList<LotteryGame> Games =
        [
            new LotteryGame("mega-sena", "Mega-Sena", 1, 60, 6, 20, "")
        ];

        public IReadOnlyList<LotteryGame> GetAll() => Games;

        public LotteryGame? GetById(string id) => Games.FirstOrDefault(x => x.Id == id);
    }

    private sealed class FakeView : IMainView
    {
        private readonly List<LanguageOptionViewModel> _languages = [];

        public event EventHandler? GenerateRequested
        {
            add { }
            remove { }
        }

        public event EventHandler? LanguageChanged;

        public event EventHandler? ExportRequested
        {
            add { }
            remove { }
        }

        public string SelectedGameId { get; private set; } = string.Empty;
        public string SelectedLanguageCode { get; private set; } = string.Empty;
        public int PicksPerPlay { get; private set; } = 6;
        public int PlayCount { get; private set; } = 1;
        public string? SelectedSpecialPick => null;

        public UiTextViewModel? LastTexts { get; private set; }

        public void BindLanguages(IReadOnlyList<LanguageOptionViewModel> languages, string selectedLanguageCode)
        {
            _languages.Clear();
            _languages.AddRange(languages);
            SelectedLanguageCode = selectedLanguageCode;
        }

        public void BindGames(IReadOnlyList<GameOptionViewModel> games, string? selectedGameId)
        {
            SelectedGameId = selectedGameId ?? games.First().Id;
        }

        public void ApplyGameRules(GameRulesViewModel rules)
        {
        }

        public void ApplyTexts(UiTextViewModel texts)
        {
            LastTexts = texts;
        }

        public void ApplySpecialPickOptions(SpecialPickOptionsViewModel? model)
        {
        }

        public void ShowError(string message)
        {
        }

        public void ShowSuccess(GenerateOutputViewModel output)
        {
        }

        public void ShowInfo(string message)
        {
        }

        public void SetGenerateEnabled(bool enabled)
        {
        }

        public string? PromptExportFilePath() => null;

        public void ChangeLanguage(string languageCode)
        {
            SelectedLanguageCode = languageCode;
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
