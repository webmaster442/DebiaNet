using System.Text.Json;

using CliWrap;

using Debianet.Domain.Dto;
using Debianet.Domain.ViewModel;
using Debianet.DomainService;
using Debianet.Infrastructure;
using Debianet.ServiceContract;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Debianet.Pages;

internal class SystemInfoModel : PageModel
{
    private readonly ICommandRunner _commandRunner;

    public SystemInfoModel(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }

    private async Task<string> RunAndGetStdout(string command, string[] arguments, int? lines, CancellationToken cancellationToken)
    {
        var result = await _commandRunner.CaptureCommandOutputAsync(command, arguments, lines, cancellationToken);
        return result.Value is string str ? str : "n/a";
    }

    private async Task<string> GetCpuData(CancellationToken cancellationToken)
    {
        var lscpuJson = await RunAndGetStdout("lscpu", ["-J"], null, cancellationToken);
        LsCpuResult? result = JsonSerializer.Deserialize<LsCpuResult>(lscpuJson, JsonSerializerOptions.Web);
        var cpuModel = result?.Lscpu.FirstOrDefault(x => x.Field == LsCpuResult.ModelName)?.Data;
        return cpuModel != null ? cpuModel : "n/a";
    }

    public SystemInfo Info { get; set; } = default!;

    public async Task OnGetAsync()
    {
        var freeOutput = await RunAndGetStdout("free", ["-h"], null, Request.HttpContext.RequestAborted);
        var dfOutput = await RunAndGetStdout("df", ["-P", "-h", "-x", "tmpfs", "-x", "overlay", "-x", "devtmpfs"], null, Request.HttpContext.RequestAborted);

        Info = new SystemInfo
        {
            HostName = Environment.MachineName,
            CpuInfo = await GetCpuData(Request.HttpContext.RequestAborted),
            KernelInfo = await RunAndGetStdout("uname", ["-s", "-r"], 1, Request.HttpContext.RequestAborted),
            UpTime = await RunAndGetStdout("uptime", ["-p"], 1, Request.HttpContext.RequestAborted),
            Shell = await RunAndGetStdout("$SHELL", ["--version"], 1, Request.HttpContext.RequestAborted),
            MemoryInfo = TableFactory.FromFreeOutput(freeOutput),
            DiskInfo = TableFactory.FromDfOutput(dfOutput)
        };
    }
}
