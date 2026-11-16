namespace Debianet.ServiceContract;

internal interface IFileSystemService
{
    public void CreateDirectories(IEnumerable<string> directories);
    string? ResolvePath(string path);
}
