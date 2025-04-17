using BookHive.Interfaces;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

[AllowAnonymous]
public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("/Books/Category/{categoryName}")]
    public async Task<IActionResult> Category(string categoryName)
    {
        var books = await _bookService.GetBooksByCategoryAsync(categoryName);

        var viewModel = new CategoryBooksViewModel
        {
            CategoryName = categoryName,
            Books = books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                IsFeatured = b.IsFeatured,
                IsDiscounted = b.IsDiscounted,
                CategoryName = b.Category?.Name ?? "N/A",
                PublishDate = b.PublishDate
            }).ToList()
        };

        return View(viewModel);
    }
}
