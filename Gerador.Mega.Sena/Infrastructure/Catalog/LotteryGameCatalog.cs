using Gerador.Mega.Sena.Application.Abstractions;
using Gerador.Mega.Sena.Domain.Entities;

namespace Gerador.Mega.Sena.Infrastructure.Catalog;

/// <summary>
/// In-memory catalog with official game limits from CAIXA pages.
/// </summary>
internal sealed class LotteryGameCatalog : ILotteryGameCatalog
{
    private static readonly IReadOnlyList<string> TimemaniaTimes =
    [
        "Flamengo", "Corinthians", "Palmeiras", "São Paulo", "Santos",
        "Grêmio", "Internacional", "Cruzeiro", "Atlético Mineiro", "Fluminense",
        "Vasco da Gama", "Botafogo", "Sport Recife", "Fortaleza", "Ceará",
        "Bahia", "Vitória", "Goiás", "América Mineiro", "Coritiba",
        "Athletico Paranaense", "Bragantino", "Avaí", "Chapecoense",
        "Náutico", "CRB", "Guarani", "Ponte Preta", "Figueirense", "ABC"
    ];

    private static readonly IReadOnlyList<string> MesesDaSorte =
    [
        "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    ];

    private static readonly IReadOnlyList<LotteryGame> Games =
    [
        new LotteryGame("mega-sena", "Mega-Sena", 1, 60, 6, 20, "Escolha de 6 a 20 numeros entre 1 e 60."),
        new LotteryGame("lotofacil", "Lotofacil", 1, 25, 15, 20, "Escolha de 15 a 20 numeros entre 1 e 25."),
        new LotteryGame("quina", "Quina", 1, 80, 5, 15, "Escolha de 5 a 15 numeros entre 1 e 80."),
        new LotteryGame("lotomania", "Lotomania", 1, 100, 50, 50, "Escolha fixa de 50 numeros entre 1 e 100."),
        new LotteryGame("dupla-sena", "Dupla Sena", 1, 50, 6, 15, "Escolha de 6 a 15 numeros entre 1 e 50."),
        new LotteryGame("timemania", "Timemania", 1, 80, 10, 10,
            "Escolha fixa de 10 numeros entre 1 e 80 com Time do Coracao.",
            "Time do Coracao", TimemaniaTimes),
        new LotteryGame("dia-de-sorte", "Dia de Sorte", 1, 31, 7, 15,
            "Escolha de 7 a 15 numeros entre 1 e 31 com Mes da Sorte.",
            "Mes da Sorte", MesesDaSorte)
    ];

    public IReadOnlyList<LotteryGame> GetAll()
    {
        return Games;
    }

    public LotteryGame? GetById(string id)
    {
        return Games.FirstOrDefault(x => x.Id == id);
    }
}
