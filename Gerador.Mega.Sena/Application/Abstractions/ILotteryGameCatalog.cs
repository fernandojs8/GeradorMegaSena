using Gerador.Mega.Sena.Domain.Entities;

namespace Gerador.Mega.Sena.Application.Abstractions;

/// <summary>
/// Exposes read-only access to configured lottery games.
/// </summary>
internal interface ILotteryGameCatalog
{
    IReadOnlyList<LotteryGame> GetAll();

    LotteryGame? GetById(string id);
}
