namespace Debianet.Abstractions;

internal interface ITerminal
{
    void SwitchToAlternateBuffer();
    void SwitchToMainBuffer();
    void Clear();
    void DisplayException(Exception ex);
    void StandardOutput(string stdOut);
    void StandardError(string stdErr);
    void FigletText(string text);
    void Info(FormattableString formattableString);
    void Warning(FormattableString formattableString);
    void Success(FormattableString formattableString);
    void Error(FormattableString formattableString);
    void WaitKey();
    void Line();
}
