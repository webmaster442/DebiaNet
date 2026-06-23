using System.Net.Sockets;

namespace Debianet.Abstractions;

internal sealed class DockerClient
{
    private const string SockerPath = "/var/run/docker.sock";
    private readonly SocketsHttpHandler _handler;
    private readonly HttpClient _client;
    private bool _disposed;

    public static bool IsDockerInstalled()
        => File.Exists(SockerPath);

    public DockerClient()
    {
        _handler = new SocketsHttpHandler
        {
            ConnectCallback = async (context, cancellationToken) =>
            {
                var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

                await socket.ConnectAsync(
                    new UnixDomainSocketEndPoint(SockerPath),
                    cancellationToken);

                return new NetworkStream(socket, ownsSocket: true);
            }
        };
        _client = new HttpClient(_handler);
        _client.BaseAddress = new Uri("http://localhost/v1.54/");
    }

    public async Task<bool> IsAccessible()
    {
        try
        {
            var response = await _client.GetAsync("_ping");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public void Dispose()
    {
        _client.Dispose();
        _handler.Dispose();
        _disposed = true;
    }
}
