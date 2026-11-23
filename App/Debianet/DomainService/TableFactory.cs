using Debianet.Domain.ViewModel;

namespace Debianet.DomainService;

internal static class TableFactory
{
    private static List<string> PadLeft(int totalCount, string[] items)
    {
        List<string> paddedList = new();
        int padCount = totalCount - items.Length;
        if (padCount > 0)
        {
            for (int i = 0; i < padCount; i++)
            {
                paddedList.Add(string.Empty);
            }
        }
        paddedList.AddRange(items);
        return paddedList;
    }

    private static List<string> PadRight(int totalCount, string[] items)
    {
        List<string> paddedList = new();
        int padCount = totalCount - items.Length;

        paddedList.AddRange(items);

        if (padCount > 0)
        {
            for (int i = 0; i < padCount; i++)
            {
                paddedList.Add(string.Empty);
            }
        }
        return paddedList;
    }

    private static List<string> Clamp(int maxCount, string[] columns) 
        => columns.Take(maxCount).ToList();

    private static IReadOnlyList<string> SplitBySpace(string str, (char start, char end)? escapeData)
    {
        if (escapeData == null)
            return str.Split([' '], StringSplitOptions.RemoveEmptyEntries);

        List<string> result = new();

        for (int i = 0; i < str.Length;)
        {
            if (str[i] == escapeData.Value.start)
            {
                int endIndex = str.IndexOf(escapeData.Value.end, i + 1);
                if (endIndex == -1)
                {
                    result.Add(str[(i + 1)..]);
                    break;
                }
                else
                {
                    result.Add(str[(i + 1)..endIndex]);
                    i = endIndex + 1;
                }
            }
            else
            {
                int nextSpace = str.IndexOf(' ', i);
                if (nextSpace == -1)
                {
                    result.Add(str[i..]);
                    break;
                }
                else
                {
                    result.Add(str[i..nextSpace]);
                    i = nextSpace + 1;
                }
            }
            while (i < str.Length && str[i] == ' ')
            {
                i++;
            }
        }

        return result;

    }

    public static Table FromDfOutput(string dfOutput)
    {
        List<TableRow> rows = new();
        string[] lines = dfOutput.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var columns = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            rows.Add(new TableRow
            {
                Columns = Clamp(6, columns)
            });
        }
        return new Table
        {
            Rows = rows,
        };
    }

    public static Table FromFreeOutput(string freeOutput)
    {
        List<TableRow> rows = new();
        string[] lines = freeOutput.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (var line in lines)
        {
            var columns = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            if (i == 0 || i == 1)
            {
                rows.Add(new TableRow
                {
                    Columns = PadLeft(7, columns)
                });
            }
            else
            {
                rows.Add(new TableRow
                {
                    Columns = PadRight(7, columns)
                });
            }
            i++;
        }
        return new Table
        {
            Rows = rows,
        };
    }

    internal static Table FromDotnetListRuntimes(string dotnetListRuntimes)
    {
        List<TableRow> rows = new();
        rows.Add(TableRow.Create("SDK", "Version", "Path"));
        string[] lines = dotnetListRuntimes.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var columns = SplitBySpace(line, ('[', ']')).ToArray();
            rows.Add(new TableRow
            {
                Columns = Clamp(3, columns)
            });
        }

        return new Table
        {
            Rows = rows,
        };
    }

    internal static Table FromDotnetListSdks(string dotnetListSdks)
    {
        List<TableRow> rows = new();
        rows.Add(TableRow.Create("Version", "Path"));
        string[] lines = dotnetListSdks.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var columns = SplitBySpace(line, ('[', ']')).ToArray();
            rows.Add(new TableRow
            {
                Columns = Clamp(2, columns)
            });
        }

        return new Table
        {
            Rows = rows,
        };
    }
}
