namespace DebiaNetApp;

internal static class ExitCodes
{
    public const int NotLinux = ushort.MaxValue;
    public const int SudoUser = 1;
    public const int Success = 0;
}