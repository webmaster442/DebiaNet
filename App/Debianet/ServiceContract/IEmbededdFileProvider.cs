namespace Debianet.ServiceContract;

internal interface IEmbededdFileProvider
{
    string GetLicenseText();
    string GetChangeLogMarkdown();
    string GetReadmeMd();
    Stream GetDataJson();
}
