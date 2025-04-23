using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookHive.Controllers;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IBookService _bookService;
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductController(IBookService bookService, DataContext context, IWebHostEnvironment env)
    {
        _bookService = bookService;
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllAsync();

        var bookViewModels = books.Select(book => new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            ImageUrl = book.ImageUrl,
            IsFeatured = book.IsFeatured,
            IsDiscounted = book.IsDiscounted,
            CategoryName = book.Category != null ? book.Category.Name : "N/A",
            PublishDate = book.PublishDate
        }).ToList();

        return View(bookViewModels);
    }

    public IActionResult Create()
    {
        var viewModel = new BookViewModel
        {
            Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookViewModel model)
    {
        Console.WriteLine("Model State: checking");
        if (!ModelState.IsValid)
        {
            model.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            return View(model);
        }
        string uniqueFileName = null;
        if (model.Image != null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(fileStream);
            }
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Price = model.Price,
                IsFeatured = model.IsFeatured,
                CategoryId = model.CategoryId,
                IsDiscounted = model.IsDiscounted,
                PublishDate = model.PublishDate,
                ImageUrl = "/images/" + uniqueFileName
            };
            await _bookService.AddAsync(book);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound();

        var viewModel = new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            ImageUrl = book.ImageUrl,
            IsFeatured = book.IsFeatured,
            CategoryId = book.CategoryId,
            IsDiscounted = book.IsDiscounted,
            PublishDate = book.PublishDate,
            Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            return View(model);
        }

        var book = await _bookService.GetByIdAsync(model.Id);
        if (book == null) return NotFound();

        if (model.Image != null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            book.ImageUrl = "/images/" + uniqueFileName;
        }

        book.Title = model.Title;
        book.Author = model.Author;
        book.Price = model.Price;
        book.IsFeatured = model.IsFeatured;
        book.CategoryId = model.CategoryId;
        book.IsDiscounted = model.IsDiscounted;
        book.PublishDate = model.PublishDate;

        await _bookService.UpdateAsync(book);

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
