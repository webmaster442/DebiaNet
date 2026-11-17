namespace Debianet.Domain;

public sealed class Result<TSuccess>
{
    private readonly TSuccess? _success;
    private readonly Error? _error;
    private readonly bool _isSuccess;

    private Result(TSuccess success)
    {
        _success = success;
        _isSuccess = true;
    }

    private Result(Error error)
    {
        _error = error;
        _isSuccess = false;
    }

    public static implicit operator Result<TSuccess>(TSuccess success)
        => new(success);

    public static implicit operator Result<TSuccess>(Error error)
        => new(error);

    public object Value => _isSuccess ? _success! : _error!;

    public override string ToString()
    {
        return _isSuccess
            ? $"Success: {_success}"
            : $"Error: {_error}";
    }
}
