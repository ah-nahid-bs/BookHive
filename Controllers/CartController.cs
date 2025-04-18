using BookHive.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookHive.Controllers;
[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var items = await _cartService.GetCartItemsAsync(userId);
        var total = await _cartService.GetCartTotalAsync(userId);

        ViewBag.CartTotal = total;
        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
    {
        var userId = GetUserId();
        await _cartService.AddToCartAsync(userId, bookId, quantity);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        await _cartService.RemoveFromCartAsync(cartItemId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        var userId = GetUserId();
        await _cartService.ClearCartAsync(userId);
        return RedirectToAction("Index");
    }
}
