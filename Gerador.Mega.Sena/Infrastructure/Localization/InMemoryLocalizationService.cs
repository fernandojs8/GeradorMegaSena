using Gerador.Mega.Sena.Application.Abstractions;

namespace Gerador.Mega.Sena.Infrastructure.Localization;

/// <summary>
/// In-memory localization provider for supported UI languages.
/// </summary>
internal sealed class InMemoryLocalizationService : ILocalizationService
{
    private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _translations;
    private readonly IReadOnlyList<LanguageOption> _languages;
    private string _currentLanguageCode;

    public InMemoryLocalizationService()
    {
        _languages =
        [
            new LanguageOption("en", "English"),
            new LanguageOption("pt", "Portugues"),
            new LanguageOption("fr", "Francais"),
            new LanguageOption("es", "Espanhol"),
            new LanguageOption("de", "Alemao")
        ];

        _translations = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["pt"] = BuildPt(),
            ["en"] = BuildEn(),
            ["fr"] = BuildFr(),
            ["es"] = BuildEs(),
            ["de"] = BuildDe()
        };

        _currentLanguageCode = DefaultLanguageCode;
    }

    public string DefaultLanguageCode => "pt";

    public IReadOnlyList<LanguageOption> GetLanguages()
    {
        return _languages;
    }

    public void SetLanguage(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            _currentLanguageCode = DefaultLanguageCode;
            return;
        }

        _currentLanguageCode = _translations.ContainsKey(languageCode)
            ? languageCode
            : DefaultLanguageCode;
    }

    public string Get(string key)
    {
        if (_translations.TryGetValue(_currentLanguageCode, out IReadOnlyDictionary<string, string>? selected)
            && selected.TryGetValue(key, out string? value))
        {
            return value;
        }

        IReadOnlyDictionary<string, string> fallback = _translations[DefaultLanguageCode];
        if (fallback.TryGetValue(key, out string? fallbackValue))
        {
            return fallbackValue;
        }

        return key;
    }

    private static IReadOnlyDictionary<string, string> BuildPt() => new Dictionary<string, string>
    {
        ["form.title"] = "Gerador de Loterias CAIXA",
        ["header.title"] = "Gerador de Jogos das Loterias",
        ["header.subtitle"] = "Escolha o jogo, informe os numeros e gere jogadas unicas em segundos.",
        ["label.language"] = "🌐 Idioma",
        ["label.game"] = "Modalidade",
        ["label.picks"] = "Quantidade de numeros",
        ["label.playCount"] = "Quantidade de jogadas:",
        ["button.generate"] = "Gerar Jogadas",
        ["results.title"] = "Jogadas Geradas",
        ["status.ready"] = "Pronto para gerar.",
        ["status.fixedTemplate"] = "Para {0}, a quantidade de numeros e fixa em {1}.",
        ["output.game"] = "Jogo",
        ["output.config"] = "Configuracao",
        ["output.warning"] = "Aviso",
        ["status.success"] = "Geradas {0} jogadas com sucesso.",
        ["status.successWarning"] = "Geradas {0} jogadas com aviso.",
        ["desc.range"] = "Escolha de {0} a {1} numeros entre {2} e {3}.",
        ["desc.fixed"] = "Escolha fixa de {0} numeros entre {1} e {2}.",
        ["msg.noGames"] = "Nenhuma modalidade foi configurada.",
        ["msg.errorGeneric"] = "Nao foi possivel gerar as jogadas.",
        ["button.export"] = "Exportar",
        ["specialPick.random"] = "Aleatorio"
    };

    private static IReadOnlyDictionary<string, string> BuildEn() => new Dictionary<string, string>
    {
        ["form.title"] = "CAIXA Lottery Generator",
        ["header.title"] = "Lottery Play Generator",
        ["header.subtitle"] = "Choose a game, set your numbers and generate unique plays in seconds.",
        ["label.language"] = "🌐 Language",
        ["label.game"] = "Game",
        ["label.picks"] = "Numbers per play",
        ["label.playCount"] = "Play count:",
        ["button.generate"] = "Generate Plays",
        ["results.title"] = "Generated Plays",
        ["status.ready"] = "Ready to generate.",
        ["status.fixedTemplate"] = "For {0}, the number of picks is fixed at {1}.",
        ["output.game"] = "Game",
        ["output.config"] = "Configuration",
        ["output.warning"] = "Warning",
        ["status.success"] = "Generated {0} plays successfully.",
        ["status.successWarning"] = "Generated {0} plays with warning.",
        ["desc.range"] = "Pick from {0} to {1} numbers between {2} and {3}.",
        ["desc.fixed"] = "Fixed pick of {0} numbers between {1} and {2}.",
        ["msg.noGames"] = "No game was configured.",
        ["msg.errorGeneric"] = "Unable to generate plays.",
        ["button.export"] = "Export",
        ["specialPick.random"] = "Random"
    };

    private static IReadOnlyDictionary<string, string> BuildFr() => new Dictionary<string, string>
    {
        ["form.title"] = "Generateur de Loteries CAIXA",
        ["header.title"] = "Generateur de Grilles",
        ["header.subtitle"] = "Choisissez le jeu, indiquez les numeros et generez des grilles uniques.",
        ["label.language"] = "🌐 Langue",
        ["label.game"] = "Jeu",
        ["label.picks"] = "Nombres par grille",
        ["label.playCount"] = "Nombre de grilles:",
        ["button.generate"] = "Generer",
        ["results.title"] = "Grilles Generees",
        ["status.ready"] = "Pret a generer.",
        ["status.fixedTemplate"] = "Pour {0}, le nombre est fixe a {1}.",
        ["output.game"] = "Jeu",
        ["output.config"] = "Configuration",
        ["output.warning"] = "Avertissement",
        ["status.success"] = "{0} grilles generees avec succes.",
        ["status.successWarning"] = "{0} grilles generees avec avertissement.",
        ["desc.range"] = "Choisissez de {0} a {1} numeros entre {2} et {3}.",
        ["desc.fixed"] = "Choix fixe de {0} numeros entre {1} et {2}.",
        ["msg.noGames"] = "Aucun jeu configure.",
        ["msg.errorGeneric"] = "Impossible de generer les grilles.",
        ["button.export"] = "Exporter",
        ["specialPick.random"] = "Aleatoire"
    };

    private static IReadOnlyDictionary<string, string> BuildEs() => new Dictionary<string, string>
    {
        ["form.title"] = "Generador de Loterias CAIXA",
        ["header.title"] = "Generador de Jugadas",
        ["header.subtitle"] = "Elige el juego, define numeros y genera jugadas unicas en segundos.",
        ["label.language"] = "🌐 Idioma",
        ["label.game"] = "Juego",
        ["label.picks"] = "Numeros por jugada",
        ["label.playCount"] = "Cantidad de jugadas:",
        ["button.generate"] = "Generar Jugadas",
        ["results.title"] = "Jugadas Generadas",
        ["status.ready"] = "Listo para generar.",
        ["status.fixedTemplate"] = "Para {0}, la cantidad de numeros es fija en {1}.",
        ["output.game"] = "Juego",
        ["output.config"] = "Configuracion",
        ["output.warning"] = "Aviso",
        ["status.success"] = "Se generaron {0} jugadas con exito.",
        ["status.successWarning"] = "Se generaron {0} jugadas con aviso.",
        ["desc.range"] = "Elige de {0} a {1} numeros entre {2} y {3}.",
        ["desc.fixed"] = "Eleccion fija de {0} numeros entre {1} y {2}.",
        ["msg.noGames"] = "No hay juego configurado.",
        ["msg.errorGeneric"] = "No se pudieron generar las jugadas.",
        ["button.export"] = "Exportar",
        ["specialPick.random"] = "Aleatorio"
    };

    private static IReadOnlyDictionary<string, string> BuildDe() => new Dictionary<string, string>
    {
        ["form.title"] = "CAIXA Lotterie Generator",
        ["header.title"] = "Generator fur Lotterie-Tipps",
        ["header.subtitle"] = "Wahlen Sie das Spiel, geben Sie Zahlen an und erzeugen Sie eindeutige Tipps.",
        ["label.language"] = "🌐 Sprache",
        ["label.game"] = "Spiel",
        ["label.picks"] = "Zahlen pro Tipp",
        ["label.playCount"] = "Anzahl Tipps:",
        ["button.generate"] = "Tipps Generieren",
        ["results.title"] = "Generierte Tipps",
        ["status.ready"] = "Bereit zum Generieren.",
        ["status.fixedTemplate"] = "Bei {0} ist die Anzahl der Zahlen fest auf {1}.",
        ["output.game"] = "Spiel",
        ["output.config"] = "Konfiguration",
        ["output.warning"] = "Hinweis",
        ["status.success"] = "{0} Tipps erfolgreich generiert.",
        ["status.successWarning"] = "{0} Tipps mit Hinweis generiert.",
        ["desc.range"] = "Wahlen Sie {0} bis {1} Zahlen zwischen {2} und {3}.",
        ["desc.fixed"] = "Feste Auswahl von {0} Zahlen zwischen {1} und {2}.",
        ["msg.noGames"] = "Kein Spiel konfiguriert.",
        ["msg.errorGeneric"] = "Tipps konnten nicht generiert werden.",
        ["button.export"] = "Exportieren",
        ["specialPick.random"] = "Zufallig"
    };
}
