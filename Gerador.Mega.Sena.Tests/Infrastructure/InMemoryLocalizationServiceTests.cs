using Gerador.Mega.Sena.Infrastructure.Localization;

namespace Gerador.Mega.Sena.Tests.Infrastructure;

public sealed class InMemoryLocalizationServiceTests
{
    [Fact]
    public void GetLanguages_ReturnsExpectedSet()
    {
        var sut = new InMemoryLocalizationService();

        var languages = sut.GetLanguages();

        Assert.Equal(5, languages.Count);
        Assert.Contains(languages, l => l.Code == "en");
        Assert.Contains(languages, l => l.Code == "pt");
        Assert.Contains(languages, l => l.Code == "fr");
        Assert.Contains(languages, l => l.Code == "es");
        Assert.Contains(languages, l => l.Code == "de");
    }

    [Fact]
    public void SetLanguage_WithEnglish_ChangesReturnedText()
    {
        var sut = new InMemoryLocalizationService();

        sut.SetLanguage("en");
        string text = sut.Get("button.generate");

        Assert.Equal("Generate Plays", text);
    }

    [Fact]
    public void SetLanguage_WithUnknownCode_FallsBackToDefault()
    {
        var sut = new InMemoryLocalizationService();

        sut.SetLanguage("zz");
        string text = sut.Get("button.generate");

        Assert.Equal("Gerar Jogadas", text);
    }
}
