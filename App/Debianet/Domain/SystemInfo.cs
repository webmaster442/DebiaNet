namespace Debianet.Domain;

internal sealed record class SystemInfo
{
    public required string HostName { get; init; }
    public required string CpuInfo { get; init; }
    public required string KernelInfo { get; init; }
    public required string UpTime { get; init; }
    public required string Shell { get; init; }
    public required string MemoryInfo { get; init; }
    public required string DiskInfo { get; init; }
}
