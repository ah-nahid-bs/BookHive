using BookHive.Interfaces;
using BookHive.ViewModels;
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
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateCartViewModel model)
    {
        Console.WriteLine("this was here");
        if (!ModelState.IsValid || model.CartItemId <= 0 || model.Quantity <= 0)
        {
            return Json(new { success = false, message = "Invalid cart item ID or quantity." });
        }

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _cartService.UpdateCartItemAsync(userId, model.CartItemId, model.Quantity);
            return Json(new { success = success, message = success ? null : "Could not update cart item." });
        }
        catch
        {
            return Json(new { success = false, message = "An error occurred while updating the cart item." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remove([FromBody] RemoveCartViewModel model)
    {
        if (!ModelState.IsValid || model.CartItemId <= 0)
        {
            return Json(new { success = false, message = "Invalid cart item ID." });
        }

        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _cartService.RemoveCartItemAsync(userId, model.CartItemId);
            return Json(new { success = success, message = success ? null : "Could not remove cart item." });
        }
        catch
        {
            return Json(new { success = false, message = "An error occurred while removing the cart item." });
        }
    }
}
