using System.Diagnostics;

using Debianet.Domain;
using Debianet.Domain.Dto;
using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class DockerService : IDockerService
{
    private readonly ICommandRunner _commandRunner;
    private readonly bool _isDockerInstalled;

    public DockerService(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
        _isDockerInstalled = File.Exists("/var/run/docker.sock");
    }

    private Result<IReadOnlyList<T>> Parse<T>(string output)
    {
        List<T> values = new List<T>();
        using var stringReader = new StringReader(output);
        while (stringReader.ReadLine() is { } line)
        {
            try
            {
                var value = System.Text.Json.JsonSerializer.Deserialize<T>(line);
                if (value is not null)
                    values.Add(value);
                else
                    return new Error($"Failed to parse JSON line: {line}");
            }
            catch (System.Text.Json.JsonException)
            {
                return new Error($"Failed to parse JSON line: {line}");
            }
        }
        return values;
    }

    public async Task<Result<IReadOnlyList<DockerImageResult>>> GetImagesAsync(CancellationToken cancellationToken)
    {
        if (!_isDockerInstalled)
            return new Error("Docker is not installed or the Docker daemon is not running.");

        var result = await _commandRunner.CaptureCommandOutputAsync(CommandBundle.Create("docker", ["images", "--format", "json"]), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string output)
            return Parse<DockerImageResult>(output);

        throw new UnreachableException("Unexpected result type from command runner.");
    }

    public async Task<Result<IReadOnlyList<DockerNetworkResult>>> GetNetworksAsync(CancellationToken cancellationToken)
    {
        if (!_isDockerInstalled)
            return new Error("Docker is not installed or the Docker daemon is not running.");

        var result = await _commandRunner.CaptureCommandOutputAsync(CommandBundle.Create("docker", ["network", "ls", "--format", "json"]), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string output)
            return Parse<DockerNetworkResult>(output);

        throw new UnreachableException("Unexpected result type from command runner.");
    }

    public async Task<Result<IReadOnlyList<DockerPsResult>>> GetPsAsync(CancellationToken cancellationToken)
    {
        if (!_isDockerInstalled)
            return new Error("Docker is not installed or the Docker daemon is not running.");

        var result = await _commandRunner.CaptureCommandOutputAsync(CommandBundle.Create("docker", ["ps", "--format", "json"]), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string output)
            return Parse<DockerPsResult>(output);

        throw new UnreachableException("Unexpected result type from command runner.");
    }

    public async Task<Result<IReadOnlyList<DockerVolumeResult>>> GetVolumesAsync(CancellationToken cancellationToken)
    {
        if (!_isDockerInstalled)
            return new Error("Docker is not installed or the Docker daemon is not running.");

        var result = await _commandRunner.CaptureCommandOutputAsync(CommandBundle.Create("docker", ["volume", "ls", "--format", "json"]), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string output)
            return Parse<DockerVolumeResult>(output);

        throw new UnreachableException("Unexpected result type from command runner.");
    }

    public async Task<Result<string>> GetComposeContainerId(string? composeFilePath, CancellationToken cancellationToken)
    {
        if (!_isDockerInstalled)
            return new Error("Docker is not installed or the Docker daemon is not running.");

        if (string.IsNullOrEmpty(composeFilePath))
            return string.Empty;

        Result<string> result = await _commandRunner.CaptureCommandOutputAsync(CommandBundle.Create("docker", ["compose", "-f", composeFilePath, "ps", "-q"]), cancellationToken);

        if (result.Value is Error error)
            return error;

        if (result.Value is string output)
            return output;

        throw new UnreachableException("Unexpected result type from command runner.");
    }
}
