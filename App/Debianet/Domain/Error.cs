namespace Debianet.Domain;

public sealed record class Error
{
    public string Message { get; }

    public Error(string message)
    {
        Message = message;
    }
}
