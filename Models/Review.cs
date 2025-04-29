using System.ComponentModel.DataAnnotations;

namespace BookHive.Models;
public class Review
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public int BookId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public ApplicationUser User { get; set; }
    public Book Book { get; set; }
}