using System.Text.Json;

using CliWrap;

using Debianet.Domain;
using Debianet.Infrastructure;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal class SystemInfoModel : PageModel
{
    private static async Task<string> RunAndGetStdout(string command, string[] arguments, int lines = -1)
    {
        if (command.StartsWith('$'))
        {
            command = Environment.GetEnvironmentVariable(command[1..]) ?? command;
        }

        await Cli.Wrap(command)
            .WithArguments(arguments)
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(StringBuilderBuffer.GetBuffer()))
            .ExecuteAsync();

        return StringBuilderBuffer.GetStringAndClear(lines);
    }

    private static async Task<string> GetCpuData()
    {
        var lscpuJson = await RunAndGetStdout("lscpu", ["-J"]);
        LsCpuResult? result = JsonSerializer.Deserialize<LsCpuResult>(lscpuJson, JsonSerializerOptions.Web);
        var cpuModel = result?.Lscpu.FirstOrDefault(x => x.Field == LsCpuResult.ModelName)?.Data;
        return cpuModel != null ? cpuModel : "n/a";
    }

    public SystemInfo Info { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Info = new SystemInfo
        {
            HostName = Environment.MachineName,
            CpuInfo = await GetCpuData(),
            KernelInfo = await RunAndGetStdout("uname", ["-s", "-r"], 1),
            UpTime = await RunAndGetStdout("uptime", ["-p"], 1),
            Shell = await RunAndGetStdout("$SHELL", ["--version"], 1),
            MemoryInfo = await RunAndGetStdout("free", ["-h"]),
            DiskInfo = await RunAndGetStdout("df", ["-h", "-x", "tmpfs", "-x", "overlay", "-x", "devtmpfs", "--output=target,size,avail,pcent"])
        };
    }
}
