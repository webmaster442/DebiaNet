namespace Debianet.Ui;

internal static class Extensions
{
    public static string FormatTime(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds < 1000)
        {
            return $"{timeSpan.TotalMilliseconds:F0} ms";
        }
        else if (timeSpan.TotalSeconds < 60)
        {
            return $"{timeSpan.TotalSeconds:F2} s";
        }
        else if (timeSpan.TotalMinutes < 60)
        {
            return $"{(int)timeSpan.TotalMinutes} minutes {timeSpan.Seconds:D2} seconds";
        }
        return $"{(int)timeSpan.TotalHours} hours {timeSpan.Minutes:D2} minutes {timeSpan.Seconds:D2} seconds";
    }
}
