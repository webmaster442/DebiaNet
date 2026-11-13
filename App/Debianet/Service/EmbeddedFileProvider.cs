using Debianet.ServiceContract;

namespace Debianet.Service;

internal sealed class EmbeddedFileProvider : IEmbededdFileProvider
{
    private readonly string[] _streams;

    public EmbeddedFileProvider()
    {
        _streams = typeof(EmbeddedFileProvider).Assembly.GetManifestResourceNames();
    }

    private Stream? GetResourceStream(string name)
    {
        string? streamName = _streams.Where(x => x.EndsWith(name)).FirstOrDefault();
        if (streamName != null)
        {
            return typeof(EmbeddedFileProvider).Assembly.GetManifestResourceStream(streamName);
        }
        return null;
    }

    private string? GetResourceAsString(string name)
    {
        Stream? stream = GetResourceStream(name);
        if (stream != null)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        return null;
    }

    public string GetChangeLogMarkdown()
    {
        return GetResourceAsString("changelog.md")
            ?? throw new InvalidOperationException("Changelog resource not found.");
    }

    public string GetLicenseText()
    {
        return GetResourceAsString("LICENCE")
            ?? throw new InvalidOperationException("License resource not found.");
    }

    public string GetReadmeMd()
    {
        return GetResourceAsString("readme.md")
            ?? throw new InvalidOperationException("Readme resource not found.");
    }

    public Stream GetDataJson()
    {
        return GetResourceStream("data.json")
            ?? throw new InvalidOperationException("Data JSON resource not found.");
    }
}
