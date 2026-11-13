namespace Debianet.Domain.ViewModel;

public sealed class Table
{
    public required List<TableRow> Rows { get; init; }

    public int RowCount
        => Rows.Count;
}
