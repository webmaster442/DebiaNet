using Debianet.Ui;

using Spectre.Console;

namespace Debianet.Abstractions;

internal sealed class Application : IApplication
{
    private sealed class ConsoleCancellationTokenSource : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ConsoleCancellationTokenSource()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        public CancellationToken Token 
            => _cancellationTokenSource.Token;

        private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            e.Cancel = true;
        }

        public void Dispose()
        {
            Console.CancelKeyPress -= OnCancelKeyPress;
            _cancellationTokenSource.Dispose();
        }
    }

    private readonly Stack<Menu> _menuStack;
    private readonly ITerminal _terminal;
    private readonly Menu _mainMenu;
    private readonly Dictionary<char, string> _icons;
    private Menu? _currentMenu;

    public Application(ITerminal terminal, MenuRegistry menuRegistry)
    {
        _menuStack = new Stack<Menu>();
        _terminal = terminal;
        _mainMenu = menuRegistry.Main;
        _icons = new Dictionary<char, string>
        {
            { 'a', "🅰" },
            { 'b', "🅱" },
            { 'c', "🅲" },
            { 'd', "🅳" },
            { 'e', "🅴" },
            { 'f', "🅵" },
            { 'g', "🅶" },
            { 'h', "🅷" },
            { 'i', "🅸" },
            { 'j', "🅹" },
            { 'k', "🅺" },
            { 'l', "🅻" },
            { 'm', "🅼" },
            { 'n', "🅽" },
            { 'o', "🅾" },
            { 'p', "🅿" },
            { 'q', "🆀" },
            { 'r', "🆁" },
            { 's', "🆂" },
            { 't', "🆃" },
            { 'u', "🆄" },
            { 'v', "🆅" },
            { 'w', "🆆" },
            { 'x', "🆇" },
            { 'y', "🆈" },
            { 'z', "🆉" }
        };
    }

    private Menu GetMenu()
    {
        return _menuStack.Count > 0
            ? _menuStack.Pop()
            : _mainMenu;
    }

    public void SwitchMenu(Menu menu)
    {
        if (_currentMenu != null)
        {
            _menuStack.Push(_currentMenu);
        }
        _menuStack.Push(menu);
    }

    public async Task Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            _currentMenu = GetMenu();
            _terminal.SwitchToAlternateBuffer();
            _terminal.Clear();
            try
            {
                _currentMenu.BeforeSelection();
                var prompt = new SelectionPrompt<MenuItemBase>()
                    .Title(_currentMenu.Title)
                    .UseConverter(MenuitemConverter)
                    .AddChoices(_currentMenu.Items);

                var grid = new Grid();
                grid.AddColumn();
                grid.AddColumn();


                prompt.SearchEnabled = true;
                var selection = AnsiConsole.Prompt(prompt);

                using (var tokenSource = new ConsoleCancellationTokenSource())
                {
                    _terminal.SwitchToMainBuffer();
                    _terminal.Clear();
                    await selection.Execute(this, tokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                _terminal.DisplayException(ex);
            }
        }
    }

    private string MenuitemConverter(MenuItemBase item)
    {
        if (item.Icon != null)
        {
            return $"{item.Icon} {item.Text}";
        }
        var firstChar = char.ToLower(item.Text[0]);
        return $"{_icons[firstChar]} {item.Text}";
    }
}
