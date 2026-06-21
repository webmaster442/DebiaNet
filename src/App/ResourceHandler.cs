namespace Debianet;

internal static class ResourceHandler
{
    private static StreamReader GetEmbeddedStream(string resourceName)
    {
        return new StreamReader(typeof(ResourceHandler).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource '{resourceName}' not found."));
    }

    public static string GetChangeLog()
    {
        using var reader = GetEmbeddedStream("Debianet.Properties.changelog.md");
        return reader.ReadToEnd();
    }

    public static string GetReadme()
    {
        using var reader = GetEmbeddedStream("Debianet.Properties.readme.md");
        return reader.ReadToEnd();
    }
}
