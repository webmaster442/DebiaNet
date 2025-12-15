namespace DebiaNetApp.Infrastructure;

internal abstract class SelectorMenuItem
{
    public SelectorMenuItem(string displayText, string icon)
    {
        DisplayText = displayText;
        Icon = icon;
    }

    public string DisplayText { get; }

    public string Icon { get; }

    public abstract Task<int> Execute();
}