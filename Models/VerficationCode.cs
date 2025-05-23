using System.ComponentModel.DataAnnotations;

namespace BookHive.Models;
public class VerificationCode
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; }

    [Required]
    public DateTime Expiry { get; set; }
}