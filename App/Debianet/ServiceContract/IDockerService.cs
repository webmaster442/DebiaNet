using Debianet.Domain;
using Debianet.Domain.Dto;

namespace Debianet.ServiceContract;

internal interface IDockerService
{
    Task<Result<IReadOnlyList<DockerPsResult>>> GetPsAsync(CancellationToken cancellationToken);
    Task<Result<IReadOnlyList<DockerImageResult>>> GetImagesAsync(CancellationToken cancellationToken);
    Task<Result<IReadOnlyList<DockerNetworkResult>>> GetNetworksAsync(CancellationToken cancellationToken);
}
