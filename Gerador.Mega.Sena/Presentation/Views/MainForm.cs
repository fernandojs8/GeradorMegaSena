using System.Drawing;
using System.Drawing.Drawing2D;
using Gerador.Mega.Sena.Presentation.Controllers;

namespace Gerador.Mega.Sena.Presentation.Views;

/// <summary>
/// WinForms implementation of the main view.
/// </summary>
internal sealed class MainForm : Form, IMainView
{
    private MainController? _controller;
    private bool _isBindingLanguages;
    private readonly Dictionary<string, GameOptionViewModel> _gamesById = new(StringComparer.Ordinal);
    private readonly Dictionary<string, LanguageOptionViewModel> _languagesByCode = new(StringComparer.OrdinalIgnoreCase);
    private UiTextViewModel _texts = new()
    {
        FormTitle = "Gerador de Loterias CAIXA",
        HeaderTitle = "Gerador de Jogos das Loterias",
        HeaderSubtitle = "Escolha o jogo, informe os numeros e gere jogadas unicas em segundos.",
        LanguageLabel = "🌐 Idioma",
        GameLabel = "Modalidade",
        PicksLabel = "Quantidade de numeros",
        PlayCountLabel = "Quantidade de jogadas:",
        GenerateButton = "Gerar Jogadas",
        ResultsTitle = "Jogadas Geradas",
        ReadyStatus = "Pronto para gerar.",
        FixedPickStatusTemplate = "Para {0}, a quantidade de numeros e fixa em {1}.",
        OutputGameLabel = "Jogo",
        OutputConfigLabel = "Configuracao",
        WarningLabel = "Aviso",
        SuccessStatusTemplate = "Geradas {0} jogadas com sucesso.",
        SuccessStatusWithWarningTemplate = "Geradas {0} jogadas com aviso.",
        DescriptionRangeTemplate = "Escolha de {0} a {1} numeros entre {2} e {3}.",
        DescriptionFixedTemplate = "Escolha fixa de {0} numeros entre {1} e {2}."
    };

    private readonly ComboBox _comboIdiomas;
    private readonly ComboBox _comboJogos;
    private readonly NumericUpDown _inputQuantidadeNumeros;
    private readonly NumericUpDown _inputQuantidadeJogadas;
    private readonly RichTextBox _painelResultados;
    private readonly Label _labelStatus;
    private readonly Label _labelRegras;
    private readonly Label _labelIdioma;
    private readonly Label _titulo;
    private readonly Label _subtitulo;
    private readonly Label _labelJogo;
    private readonly Label _labelNumeros;
    private readonly Label _labelJogadas;
    private readonly Label _tituloResultados;
    private readonly Button _botaoGerar;

    public event EventHandler? GenerateRequested;

    public event EventHandler? LanguageChanged;

    public string SelectedGameId
    {
        get
        {
            if (_comboJogos.SelectedItem is ComboItem item)
            {
                return item.Id;
            }

            return string.Empty;
        }
    }

    public string SelectedLanguageCode
    {
        get
        {
            if (_comboIdiomas.SelectedItem is ComboItem item)
            {
                return item.Id;
            }

            return string.Empty;
        }
    }

    public int PicksPerPlay => (int)_inputQuantidadeNumeros.Value;

    public int PlayCount => (int)_inputQuantidadeJogadas.Value;

    public MainForm()
    {
        Text = _texts.FormTitle;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(980, 660);
        BackColor = Color.FromArgb(240, 244, 247);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        DoubleBuffered = true;

        var faixaTopo = new GradientPanel
        {
            Dock = DockStyle.Top,
            Height = 138,
            CorInicial = Color.FromArgb(13, 52, 78),
            CorFinal = Color.FromArgb(10, 124, 138)
        };

        _titulo = new Label
        {
            Text = _texts.HeaderTitle,
            Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point),
            AutoSize = true,
            ForeColor = Color.White,
            Location = new Point(22, 18),
            BackColor = Color.Transparent
        };

        _subtitulo = new Label
        {
            Text = _texts.HeaderSubtitle,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
            AutoSize = false,
            ForeColor = Color.FromArgb(220, 241, 246),
            Location = new Point(24, 62),
            Size = new Size(560, 48),
            BackColor = Color.Transparent
        };

        _labelIdioma = new Label
        {
            Text = _texts.LanguageLabel,
            AutoSize = true,
            ForeColor = Color.FromArgb(230, 245, 250),
            Location = new Point(730, 18),
            BackColor = Color.Transparent,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point)
        };

        _comboIdiomas = new ComboBox
        {
            Location = new Point(732, 42),
            Size = new Size(210, 31),
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White
        };
        _comboIdiomas.SelectedIndexChanged += (_, _) =>
        {
            if (!_isBindingLanguages)
            {
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }
        };

        faixaTopo.Controls.Add(_titulo);
        faixaTopo.Controls.Add(_subtitulo);
        faixaTopo.Controls.Add(_labelIdioma);
        faixaTopo.Controls.Add(_comboIdiomas);

        var cardEntrada = new Panel
        {
            Location = new Point(24, 154),
            Size = new Size(804, 202),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        _labelJogo = new Label
        {
            Text = _texts.GameLabel,
            AutoSize = true,
            Location = new Point(22, 20),
            ForeColor = Color.FromArgb(34, 52, 70)
        };

        _comboJogos = new ComboBox
        {
            Location = new Point(24, 44),
            Size = new Size(320, 31),
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat
        };
        _comboJogos.SelectedIndexChanged += OnGameSelectionChanged;

        _labelNumeros = new Label
        {
            Text = _texts.PicksLabel,
            AutoSize = true,
            Location = new Point(364, 20),
            ForeColor = Color.FromArgb(34, 52, 70)
        };

        _inputQuantidadeNumeros = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 100,
            Value = 6,
            Location = new Point(366, 44),
            Size = new Size(180, 30),
            BorderStyle = BorderStyle.FixedSingle
        };

        _labelJogadas = new Label
        {
            Text = _texts.PlayCountLabel,
            AutoSize = true,
            Location = new Point(566, 20),
            ForeColor = Color.FromArgb(34, 52, 70)
        };

        _inputQuantidadeJogadas = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 100000,
            Value = 5,
            Location = new Point(568, 44),
            Size = new Size(212, 30),
            BorderStyle = BorderStyle.FixedSingle
        };

        _labelRegras = new Label
        {
            Text = string.Empty,
            AutoSize = false,
            Location = new Point(22, 88),
            Size = new Size(758, 28),
            ForeColor = Color.FromArgb(56, 88, 109),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point)
        };

        _botaoGerar = new Button
        {
            Text = _texts.GenerateButton,
            Location = new Point(568, 128),
            Size = new Size(212, 42),
            BackColor = Color.FromArgb(14, 140, 93),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point)
        };
        _botaoGerar.FlatAppearance.BorderSize = 0;
        _botaoGerar.Click += (_, _) => GenerateRequested?.Invoke(this, EventArgs.Empty);

        _labelStatus = new Label
        {
            Text = _texts.ReadyStatus,
            AutoSize = true,
            Location = new Point(22, 138),
            ForeColor = Color.FromArgb(74, 95, 113),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point)
        };

        var cardResultados = new Panel
        {
            Location = new Point(24, 376),
            Size = new Size(804, 214),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        _tituloResultados = new Label
        {
            Text = _texts.ResultsTitle,
            AutoSize = true,
            Location = new Point(16, 14),
            ForeColor = Color.FromArgb(31, 49, 70),
            Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        cardEntrada.Controls.Add(_labelNumeros);
        cardEntrada.Controls.Add(_inputQuantidadeNumeros);
        cardEntrada.Controls.Add(_labelJogo);
        cardEntrada.Controls.Add(_comboJogos);
        cardEntrada.Controls.Add(_labelJogadas);
        cardEntrada.Controls.Add(_inputQuantidadeJogadas);
        cardEntrada.Controls.Add(_labelRegras);
        cardEntrada.Controls.Add(_botaoGerar);
        cardEntrada.Controls.Add(_labelStatus);

        _painelResultados = new RichTextBox
        {
            Location = new Point(16, 44),
            Size = new Size(770, 150),
            ReadOnly = true,
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(248, 252, 255),
            ForeColor = Color.FromArgb(29, 46, 64),
            Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        cardResultados.Controls.Add(_tituloResultados);
        cardResultados.Controls.Add(_painelResultados);

        Controls.Add(faixaTopo);
        Controls.Add(cardEntrada);
        Controls.Add(cardResultados);
    }

    public void AttachController(MainController controller)
    {
        _controller = controller;
        _controller.Initialize();
    }

    public void BindLanguages(IReadOnlyList<LanguageOptionViewModel> languages, string selectedLanguageCode)
    {
        _isBindingLanguages = true;

        _languagesByCode.Clear();
        _comboIdiomas.Items.Clear();

        foreach (LanguageOptionViewModel language in languages)
        {
            _languagesByCode[language.Code] = language;
            _comboIdiomas.Items.Add(new ComboItem(language.Code, language.DisplayName));
        }

        int index = _comboIdiomas.Items
            .Cast<ComboItem>()
            .ToList()
            .FindIndex(x => string.Equals(x.Id, selectedLanguageCode, StringComparison.OrdinalIgnoreCase));

        _comboIdiomas.SelectedIndex = index >= 0 ? index : 0;
        _isBindingLanguages = false;
    }

    public void BindGames(IReadOnlyList<GameOptionViewModel> games, string? selectedGameId)
    {
        _gamesById.Clear();
        _comboJogos.Items.Clear();

        foreach (GameOptionViewModel game in games)
        {
            _gamesById[game.Id] = game;
            _comboJogos.Items.Add(new ComboItem(game.Id, game.Name));
        }

        if (_comboJogos.Items.Count > 0)
        {
            int selectedIndex = _comboJogos.Items
                .Cast<ComboItem>()
                .ToList()
                .FindIndex(x => string.Equals(x.Id, selectedGameId, StringComparison.Ordinal));

            _comboJogos.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
        }
    }

    public void ApplyTexts(UiTextViewModel texts)
    {
        _texts = texts;
        Text = texts.FormTitle;
        _titulo.Text = texts.HeaderTitle;
        _subtitulo.Text = texts.HeaderSubtitle;
        _labelIdioma.Text = texts.LanguageLabel;
        _labelJogo.Text = texts.GameLabel;
        _labelNumeros.Text = texts.PicksLabel;
        _labelJogadas.Text = texts.PlayCountLabel;
        _botaoGerar.Text = texts.GenerateButton;
        _tituloResultados.Text = texts.ResultsTitle;
        _labelStatus.Text = texts.ReadyStatus;
    }

    public void ApplyGameRules(GameRulesViewModel rules)
    {
        _labelRegras.Text = rules.Description;
        _inputQuantidadeNumeros.Minimum = rules.MinPicks;
        _inputQuantidadeNumeros.Maximum = rules.MaxPicks;
        _inputQuantidadeNumeros.Value = rules.MinPicks;
        _inputQuantidadeNumeros.Enabled = !rules.HasFixedPickCount;
    }

    public void ShowError(string message)
    {
        _labelStatus.ForeColor = Color.FromArgb(168, 34, 34);
        _labelStatus.Text = message;
        _painelResultados.Clear();
    }

    public void ShowSuccess(GenerateOutputViewModel output)
    {
        _painelResultados.Clear();
        _painelResultados.AppendText($"{_texts.OutputGameLabel}: {output.GameName}\n");
        _painelResultados.AppendText($"{_texts.OutputConfigLabel}: {output.PicksPerPlay}\n");
        _painelResultados.AppendText("------------------------------------------------------------\n");

        for (int i = 0; i < output.Plays.Count; i++)
        {
            _painelResultados.AppendText($"{i + 1:00}) {output.Plays[i]}\n");
        }

        _labelStatus.ForeColor = Color.FromArgb(14, 140, 93);
        _labelStatus.Text = output.Warning is null
            ? string.Format(_texts.SuccessStatusTemplate, output.Plays.Count)
            : string.Format(_texts.SuccessStatusWithWarningTemplate, output.Plays.Count);

        if (output.Warning is not null)
        {
            _painelResultados.AppendText($"\n{_texts.WarningLabel}: {output.Warning}");
        }
    }

    public void ShowInfo(string message)
    {
        _labelStatus.ForeColor = Color.FromArgb(74, 95, 113);
        _labelStatus.Text = message;
    }

    public void SetGenerateEnabled(bool enabled)
    {
        _botaoGerar.Enabled = enabled;
    }

    private void OnGameSelectionChanged(object? sender, EventArgs e)
    {
        if (_comboJogos.SelectedItem is not ComboItem item)
        {
            return;
        }

        _controller?.ApplySelectedGameRules(item.Id);
    }
}

/// <summary>
/// Decorative panel used to paint a diagonal gradient header.
/// </summary>
internal sealed class GradientPanel : Panel
{
    public Color CorInicial { get; set; } = Color.Black;

    public Color CorFinal { get; set; } = Color.DimGray;

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        using var brush = new LinearGradientBrush(ClientRectangle, CorInicial, CorFinal, LinearGradientMode.ForwardDiagonal);
        e.Graphics.FillRectangle(brush, ClientRectangle);
    }
}

/// <summary>
/// Simple combo model for id-name binding without exposing domain objects to WinForms controls.
/// </summary>
internal sealed class ComboItem
{
    public ComboItem(string id, string text)
    {
        Id = id;
        Text = text;
    }

    public string Id { get; }

    public string Text { get; }

    public override string ToString()
    {
        return Text;
    }
}
