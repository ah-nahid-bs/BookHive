using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHive.ViewModels;

public class BookViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public IFormFile? Image { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsFeatured { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public DateOnly PublishDate { get; set; }

    public bool IsDiscounted { get; set; }

    public IEnumerable<SelectListItem>? Categories { get; set; }
}
