using System.Text.Json;

using CliWrap;

using Debianet.Dto;
using Debianet.Properties;

namespace Debianet.Abstractions;

internal static class SystemInfoCollector
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



    public static async IAsyncEnumerable<(string property, string value)> CollectSystemInfoAsync()
    {
        yield return (Resources.SysInfo_HostName, Environment.MachineName);
        yield return (Resources.SysInfo_Cpu, await GetCpuData());
        yield return (Resources.SysInfo_KernelVersion, await RunAndGetStdout("uname", ["-s", "-r"], 1));
        yield return (Resources.SysInfo_Uptime, await RunAndGetStdout("uptime", ["-p"], 1));
        yield return (Resources.SysInfo_Shell, await RunAndGetStdout("$SHELL", ["--version"], 1));
        yield return (Resources.SysInfo_Memory, await RunAndGetStdout("free", ["-h"]));
        yield return (Resources.SysInfo_Disks, await RunAndGetStdout("df", ["-h", "-x", "tmpfs", "-x", "overlay", "-x", "devtmpfs", "--output=target,size,avail,pcent"]));
    }

    private static async Task<string> GetCpuData()
    {
        var lscpuJson = await RunAndGetStdout("lscpu", ["-J"]);
        LsCpuResult? result = JsonSerializer.Deserialize<LsCpuResult>(lscpuJson, JsonSerializerOptions.Web);
        var cpuModel = result?.Lscpu.FirstOrDefault(x => x.Field == LsCpuResult.ModelName)?.Data;
        return cpuModel != null ? cpuModel : "n/a";
    }
}

