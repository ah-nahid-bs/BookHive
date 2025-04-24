using BookHive.Interfaces;
using BookHive.ViewModels;
using BookHive.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookHive.Controllers;

[Authorize]
[Route("[controller]")]
public class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;
    private readonly DataContext _context;

    public WishlistController(IWishlistService wishlistService, DataContext context)
    {
        _wishlistService = wishlistService;
        _context = context;
    }

    [HttpPost("AddToWishlist")]
    public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistViewModel model)
    {
        if (!ModelState.IsValid || model.BookId <= 0)
        {
            return Json(new { success = false, message = "Invalid book ID." });
        }

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _wishlistService.AddToWishlistAsync(userId, model.BookId);

            return Json(new { success = success, message = success ? null : "Book is already in your wishlist or does not exist." });
        }
        catch
        {
            return Json(new { success = false, message = "An error occurred while adding the book to the wishlist." });
        }
    }

    [HttpPost("RemoveFromWishlist")]
    public async Task<IActionResult> RemoveFromWishlist([FromBody] AddToWishlistViewModel model)
    {
        if (!ModelState.IsValid || model.BookId <= 0)
        {
            return Json(new { success = false, message = "Invalid book ID." });
        }

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _wishlistService.RemoveFromWishlistAsync(userId, model.BookId);

            return Json(new { success = success, message = success ? null : "Book not found in wishlist." });
        }
        catch
        {
            return Json(new { success = false, message = "An error occurred while removing the book from the wishlist." });
        }
    }

    [HttpGet("GetWishlistBookIds")]
    public async Task<IActionResult> GetWishlistBookIds()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookIds = await _wishlistService.GetWishlistBookIdsAsync(userId);
            return Json(bookIds);
        }
        catch
        {
            return Json(new { success = false, message = "An error occurred while retrieving the wishlist." });
        }
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookIds = await _wishlistService.GetWishlistBookIdsAsync(userId);

            var wishlistItems = await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .Select(b => new WishlistViewModel
                {
                    BookId = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    ImageUrl = b.ImageUrl
                })
                .ToListAsync();

            return View(wishlistItems);
        }
        catch
        {
            return View(new List<WishlistViewModel>());
        }
    }
}
