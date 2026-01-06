using System.ComponentModel.DataAnnotations;

namespace PixivDownloaderX.Utilities;

public class NumericValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        return ulong.TryParse(value?.ToString(), out _)
            ? ValidationResult.Success
            : new ValidationResult("不是数字");
    }
}