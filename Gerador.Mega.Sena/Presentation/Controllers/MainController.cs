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
    private readonly GeneratePlaysUseCase _generatePlaysUseCase;
    private IReadOnlyList<GameOptionViewModel> _games = [];

    public MainController(IMainView view, ILotteryGameCatalog catalog, GeneratePlaysUseCase generatePlaysUseCase)
    {
        _view = view;
        _catalog = catalog;
        _generatePlaysUseCase = generatePlaysUseCase;

        _view.GenerateRequested += OnGenerateRequested;
    }

    public void Initialize()
    {
        _games = _catalog
            .GetAll()
            .Select(game => new GameOptionViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                MinPicks = game.MinPicks,
                MaxPicks = game.MaxPicks
            })
            .ToList();

        _view.BindGames(_games);

        var first = _games.FirstOrDefault();
        if (first is null)
        {
            _view.ShowError("Nenhuma modalidade foi configurada.");
            _view.SetGenerateEnabled(false);
            return;
        }

        ApplyRules(first);
        _view.ShowInfo("Pronto para gerar.");
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
            _view.ShowInfo($"Para {game.Name}, a quantidade de numeros e fixa em {game.MinPicks}.");
        }
        else
        {
            _view.ShowInfo("Pronto para gerar.");
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
                _view.ShowError(result.Error ?? "Nao foi possivel gerar as jogadas.");
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
}
