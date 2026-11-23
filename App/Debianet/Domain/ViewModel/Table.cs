using System.Text;
using System.Web;

namespace Debianet.Domain.ViewModel;

public sealed class Table
{
    public required List<TableRow> Rows { get; init; }

    public int RowCount
        => Rows.Count;

    public string ToHtmlTable(bool firstRowHeader = true)
    {
        StringBuilder html = new StringBuilder();
        html.AppendLine("<table>");
        foreach (var row in Rows.Index())
        {
            if (row.Index == 0 && firstRowHeader)
            {
                html.AppendLine("<thead>");
                html.AppendLine("<tr>");
                foreach (var column in row.Item.Columns)
                {
                    html.AppendLine($"<th>{HttpUtility.HtmlEncode(column)}</th>");
                }
                html.AppendLine("</tr>");
                html.AppendLine("</thead>");
            }
            else
            {
                if (firstRowHeader)
                    html.AppendLine("<tbody>");

                html.AppendLine("<tr>");
                foreach (var column in row.Item.Columns)
                {
                    html.AppendLine($"<td>{HttpUtility.HtmlEncode(column)}</td>");
                }
                html.AppendLine("</tr>");
            }
        }

        if (firstRowHeader)
            html.AppendLine("</tbody>");

        html.AppendLine("</table>");
        return html.ToString();
    }
}
