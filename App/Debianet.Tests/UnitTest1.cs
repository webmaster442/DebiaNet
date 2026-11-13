using Debianet.Domain.ViewModel;
using Debianet.DomainService;

namespace Debianet.Tests;

public class Tests
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
}
