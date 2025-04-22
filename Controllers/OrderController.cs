using BookHive.Interfaces;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookHive.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = GetUserId();
        var cartItems = await _cartService.GetCartItemsAsync(userId);
        if (!cartItems.Any())
            return RedirectToAction("Index", "Cart");

        var model = new CheckoutViewModel
        {
            CartItems = cartItems,
            TotalAmount = await _cartService.GetCartTotalAsync(userId)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder()
    {
        var userId = GetUserId();
        try
        {
            await _orderService.CreateOrderAsync(userId);
            return RedirectToAction("OrderConfirmation");
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Cart");
        }
    }

    [HttpGet]
    public IActionResult OrderConfirmation()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var userId = GetUserId();
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetOrderDetailsAsync(id);
        if (order == null)
            return NotFound();

        return View(order);
    }
}