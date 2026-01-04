using System.ComponentModel.DataAnnotations;

namespace PixivDownloaderX.Utilities;

public class ArtworkIdValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return uint.TryParse(value?.ToString(), out _)
            ? ValidationResult.Success
            : new ValidationResult("Invalid Artwork Id");
    }
}