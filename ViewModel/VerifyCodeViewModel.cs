using System.ComponentModel.DataAnnotations;

namespace BookHive.ViewModels;
public class VerifyCodeViewModel
{
    [Required(ErrorMessage = "The verification code is required.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "The verification code must be exactly 6 digits.")]
    public string Code { get; set; }
}