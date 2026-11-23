namespace Debianet.Domain.ViewModel;

public sealed class TableRow
{
    public required List<string> Columns { get; init; }

    internal static TableRow Create(params List<string> columns)
    {
        return new TableRow
        {
            Columns = columns
        };
    }
}
