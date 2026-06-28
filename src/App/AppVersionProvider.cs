namespace Debianet;

internal static class AppVersionProvider
{
    public static Version GetAppVersion()
    {
        Version? version = typeof(AppVersionProvider)
            .Assembly.GetName().Version;

        return version ?? new Version(0, 0, 0, 0);
    }
}
