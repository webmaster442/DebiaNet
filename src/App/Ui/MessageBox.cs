using Debianet.Abstractions;

namespace Debianet.Ui;

internal sealed class MessageBox
{
    public required string Title { get; set; }
    public required string Message { get; set; }
}
