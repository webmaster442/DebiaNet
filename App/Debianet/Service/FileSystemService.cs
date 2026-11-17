using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class FileSystemService : IFileSystemService
{
    private readonly string _home;
    private readonly string _appData;
    private readonly ILogger<FileSystemService> _logger;

    public FileSystemService(ILoggerFactory loggerFactory)
    {
        _home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        _appData = Path.Combine(AppContext.BaseDirectory, "Data");
        _logger = loggerFactory.CreateLogger<FileSystemService>();
    }

    public void CreateDirectories(IEnumerable<string> directories)
    {
        foreach (var directory in directories)
        {
            var resolvedPath = ResolvePath(directory);
            if (resolvedPath == null)
            {
                _logger.LogWarning("Could not resolve path for directory: {Directory}", directory);
            }
            else if (!Directory.Exists(resolvedPath))
            {
                Directory.CreateDirectory(resolvedPath);
            }
        }
    }

    public bool FileExists(string path)
    {
        var resolvedPath = ResolvePath(path);
        return resolvedPath != null && File.Exists(resolvedPath);
    }

    public string? ResolvePath(string path)
    {
        if (path.StartsWith("~")) {
            return Path.Combine(_home, path.Substring(1).TrimStart(Path.DirectorySeparatorChar));
        }
        else if (path.StartsWith("$appData"))
        {
            return Path.Combine(_appData, path.Substring("$appData".Length).TrimStart(Path.DirectorySeparatorChar));
        }
        return null;
    }
}
