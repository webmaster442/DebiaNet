namespace Debianet.ServiceContract;

public interface IEmbededdFileProvider
{
    string GetLicenseText();
    string GetChangeLogMarkdown();
    string GetReadmeMd();
}
