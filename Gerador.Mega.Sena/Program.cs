using Gerador.Mega.Sena.Application.UseCases;
using Gerador.Mega.Sena.Domain.Services;
using Gerador.Mega.Sena.Infrastructure.Catalog;
using Gerador.Mega.Sena.Infrastructure.Localization;
using Gerador.Mega.Sena.Presentation.Controllers;
using Gerador.Mega.Sena.Presentation.Views;

namespace Gerador.Mega.Sena;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var catalog = new LotteryGameCatalog();
        var localization = new InMemoryLocalizationService();
        var playGenerator = new UniquePlayGenerator();
        var useCase = new GeneratePlaysUseCase(catalog, playGenerator);

        var form = new MainForm();
        var controller = new MainController(form, catalog, localization, useCase);

        form.AttachController(controller);
        System.Windows.Forms.Application.Run(form);
    }
}
