using System.ComponentModel.DataAnnotations;

namespace Debianet.Domain.ViewModel;

public sealed record class DonetToolSelectionViewModel
{
    [Required]
    public required string Name { get; set; }

    public bool IsChecked { get; set; }
}