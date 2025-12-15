namespace DebiaNetApp.Infrastructure;

internal sealed class DelegateMenuItem : SelectorMenuItem
{
    public DelegateMenuItem(string displayText, string icon, Func<Task<int>> delegateTask) : base(displayText, icon)
    {
        DelegateTask = delegateTask;
    }

    public Func<Task<int>> DelegateTask { get; }

    public override async Task<int> Execute()
        => await DelegateTask.Invoke();
}
