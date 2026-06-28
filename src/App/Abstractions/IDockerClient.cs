namespace Debianet.Abstractions;

internal interface IDockerClient
{
    Task<bool> IsAccessible();
}