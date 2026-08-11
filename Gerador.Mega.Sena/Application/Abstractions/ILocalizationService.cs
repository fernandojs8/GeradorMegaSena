namespace Gerador.Mega.Sena.Application.Abstractions;

/// <summary>
/// Provides localized text and language metadata for the UI layer.
/// </summary>
internal interface ILocalizationService
{
    string DefaultLanguageCode { get; }

    IReadOnlyList<LanguageOption> GetLanguages();

    void SetLanguage(string languageCode);

    string Get(string key);
}

/// <summary>
/// Represents a selectable language option.
/// </summary>
internal sealed class LanguageOption
{
    public LanguageOption(string code, string displayName)
    {
        Code = code;
        DisplayName = displayName;
    }

    public string Code { get; }

    public string DisplayName { get; }
}
