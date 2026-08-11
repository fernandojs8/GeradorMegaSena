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
    private readonly Dictionary<string, GameOptionViewModel> _gamesById = new(StringComparer.Ordinal);
    private readonly ComboBox _comboJogos;
    private readonly NumericUpDown _inputQuantidadeNumeros;
    private readonly NumericUpDown _inputQuantidadeJogadas;
    private readonly RichTextBox _painelResultados;
    private readonly Label _labelStatus;
    private readonly Label _labelRegras;
    private readonly Button _botaoGerar;

    public event EventHandler? GenerateRequested;

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

    public int PicksPerPlay => (int)_inputQuantidadeNumeros.Value;

    public int PlayCount => (int)_inputQuantidadeJogadas.Value;

    public MainForm()
    {
        Text = "Gerador de Loterias CAIXA";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(860, 600);
        BackColor = Color.FromArgb(240, 244, 247);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        DoubleBuffered = true;

        var faixaTopo = new GradientPanel
        {
            Dock = DockStyle.Top,
            Height = 112,
            CorInicial = Color.FromArgb(13, 52, 78),
            CorFinal = Color.FromArgb(10, 124, 138)
        };

        var titulo = new Label
        {
            Text = "Gerador de Jogos das Loterias",
            Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point),
            AutoSize = true,
            ForeColor = Color.White,
            Location = new Point(22, 18),
            BackColor = Color.Transparent
        };

        var subtitulo = new Label
        {
            Text = "Escolha o jogo, informe os numeros e gere jogadas unicas em segundos.",
            Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point),
            AutoSize = true,
            ForeColor = Color.FromArgb(220, 241, 246),
            Location = new Point(24, 62),
            BackColor = Color.Transparent
        };

        faixaTopo.Controls.Add(titulo);
        faixaTopo.Controls.Add(subtitulo);

        var cardEntrada = new Panel
        {
            Location = new Point(24, 128),
            Size = new Size(804, 202),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var labelJogo = new Label
        {
            Text = "Modalidade",
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

        var labelNumeros = new Label
        {
            Text = "Quantidade de numeros",
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

        var labelJogadas = new Label
        {
            Text = "Quantidade de jogadas:",
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
            Text = "Gerar Jogadas",
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
            Text = "Pronto para gerar.",
            AutoSize = true,
            Location = new Point(22, 138),
            ForeColor = Color.FromArgb(74, 95, 113),
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point)
        };

        var cardResultados = new Panel
        {
            Location = new Point(24, 350),
            Size = new Size(804, 214),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        var tituloResultados = new Label
        {
            Text = "Jogadas Geradas",
            AutoSize = true,
            Location = new Point(16, 14),
            ForeColor = Color.FromArgb(31, 49, 70),
            Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point)
        };

        cardEntrada.Controls.Add(labelNumeros);
        cardEntrada.Controls.Add(_inputQuantidadeNumeros);
        cardEntrada.Controls.Add(labelJogo);
        cardEntrada.Controls.Add(_comboJogos);
        cardEntrada.Controls.Add(labelJogadas);
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

        cardResultados.Controls.Add(tituloResultados);
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

    public void BindGames(IReadOnlyList<GameOptionViewModel> games)
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
            _comboJogos.SelectedIndex = 0;
        }
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
        _painelResultados.AppendText($"Jogo: {output.GameName}\n");
        _painelResultados.AppendText($"Configuracao: {output.PicksPerPlay} numero(s) por jogada\n");
        _painelResultados.AppendText("------------------------------------------------------------\n");

        for (int i = 0; i < output.Plays.Count; i++)
        {
            _painelResultados.AppendText($"{i + 1:00}) {output.Plays[i]}\n");
        }

        _labelStatus.ForeColor = Color.FromArgb(14, 140, 93);
        _labelStatus.Text = output.Warning is null
            ? $"Geradas {output.Plays.Count} jogadas com sucesso."
            : $"Geradas {output.Plays.Count} jogadas com aviso.";

        if (output.Warning is not null)
        {
            _painelResultados.AppendText($"\nAviso: {output.Warning}");
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
