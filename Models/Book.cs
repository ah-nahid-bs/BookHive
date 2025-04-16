namespace BookHive.Models;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public int CategoryId { get; set; }
    public DateOnly PublishDate { get; set; }
    public int TotalSold { get; set; }
    public bool IsDiscounted { get; set; } = false;



    public Category? Category { get; set; }
}
