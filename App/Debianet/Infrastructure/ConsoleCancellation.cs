namespace Debianet.Infrastructure;

public sealed class ConsoleCancellation : IDisposable
{
    private bool _disposed;
    private readonly CancellationTokenSource _tokenSource;

    public CancellationToken Token => _tokenSource.Token;

    public ConsoleCancellation()
    {
        _tokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += OnCancelKeyPress;
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        _tokenSource.Cancel();
        e.Cancel = true;
    }

    public void Dispose()
    {
        _tokenSource.Dispose();
        _disposed = true;
    }
}
