using BookHive.Interfaces;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
        {
            await _cartService.AddToCartAsync(bookId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateCartViewModel model)
        {
            if (!ModelState.IsValid || model.CartItemId <= 0 || model.Quantity <= 0)
            {
                return Json(new { success = false, message = "Invalid cart item ID or quantity." });
            }

            var success = await _cartService.UpdateCartItemAsync(model.CartItemId, model.Quantity);
            return Json(new { success, message = success ? null : "Could not update cart item." });
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] RemoveCartViewModel model)
        {
            if (!ModelState.IsValid || model.CartItemId <= 0)
            {
                return Json(new { success = false, message = "Invalid cart item ID." });
            }

            var success = await _cartService.RemoveCartItemAsync(model.CartItemId);
            return Json(new { success, message = success ? null : "Could not remove cart item." });
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            return RedirectToAction("Index");
        }
    }
}