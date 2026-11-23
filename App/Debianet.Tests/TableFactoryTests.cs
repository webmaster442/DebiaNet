using Debianet.Domain.ViewModel;
using Debianet.DomainService;

namespace Debianet.Tests;

[TestFixture]
public class TableFactoryTests
{
    private const string DfOutput = """
        Filesystem      Size  Used Avail Use% Mounted on
        drivers         232G  174G   59G  75% /usr/lib/wsl/drivers
        /dev/sdd       1007G   17G  940G   2% /
        rootfs          7.8G  2.8M  7.8G   1% /init
        C:\             232G  174G   59G  75% /mnt/c
        D:\             1.8T  1.1T  747G  60% /mnt/d
        E:\              30G   16G   15G  51% /mnt/e
        F:\             895G  328G  567G  37% /mnt/f
        P:\              20G  4.4G   16G  22% /mnt/p
        Z:\              20G  9.9G   11G  50% /mnt/z
        """;

    private const string FreeCommandOutput = """
                       total        used        free      shared  buff/cache   available
        Mem:            15Gi       574Mi        14Gi       3.5Mi       648Mi        15Gi
        Swap:          4.0Gi          0B       4.0Gi
        """;

    private const string DotnetListRuntimes = """
        Microsoft.AspNetCore.App 8.0.34 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
        Microsoft.AspNetCore.App 9.0.23 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
        Microsoft.AspNetCore.App 10.0.15 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
        Microsoft.AspNetCore.App 11.0.0-preview.6.26359.118 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
        Microsoft.AspNetCore.App 11.0.0 [C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App]
        Microsoft.NETCore.App 8.0.25 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
        Microsoft.NETCore.App 8.0.34 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
        Microsoft.NETCore.App 9.0.23 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
        Microsoft.NETCore.App 10.0.15 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
        Microsoft.NETCore.App 11.0.0-preview.6.26359.118 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
        Microsoft.NETCore.App 11.0.0 [C:\Program Files\dotnet\shared\Microsoft.NETCore.App]
        Microsoft.WindowsDesktop.App 8.0.34 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
        Microsoft.WindowsDesktop.App 9.0.23 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
        Microsoft.WindowsDesktop.App 10.0.15 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
        Microsoft.WindowsDesktop.App 11.0.0-preview.6.26359.118 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
        Microsoft.WindowsDesktop.App 11.0.0 [C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App]
        """;

    [Test]
    public void TableFactory_FromFreeOutput_ReturnsTable()
    {
        Table table = TableFactory.FromFreeOutput(FreeCommandOutput);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(table.RowCount, Is.EqualTo(3));
            Assert.That(table.Rows.Min(r => r.Columns.Count), Is.EqualTo(7));
        }
    }

    [Test]
    public void TableFactory_FromDfOutput_ReturnsTable()
    {
        Table table = TableFactory.FromDfOutput(DfOutput);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(table.RowCount, Is.EqualTo(10));
            Assert.That(table.Rows.Min(r => r.Columns.Count), Is.EqualTo(6));
        }
    }

    [Test]
    public void TableFactory_FromDotnetListRuntimes_Returns_Table()
    {
        Table table = TableFactory.FromDotnetListRuntimes(DotnetListRuntimes);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(table.RowCount, Is.EqualTo(17));
            Assert.That(table.Rows.Min(r => r.Columns.Count), Is.EqualTo(3));
        }
    }
}
