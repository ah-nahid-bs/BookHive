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
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetBookDetailsAsync(id);
        if (book == null)
            return NotFound();

        var viewModel = new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            ImageUrl = book.ImageUrl,
            IsFeatured = book.IsFeatured,
            IsDiscounted = book.IsDiscounted,
            CategoryName = book.Category?.Name ?? "N/A",
            PublishDate = book.PublishDate
        };

        return View(viewModel);
    }
}
