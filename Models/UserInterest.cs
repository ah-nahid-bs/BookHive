namespace BookHive.Models;
public class UserInterest
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string InterestType { get; set; } 
    public int? CategoryId { get; set; }
    public string? AuthorName { get; set; }

    public ApplicationUser User { get; set; }
    public Category Category { get; set; }
}