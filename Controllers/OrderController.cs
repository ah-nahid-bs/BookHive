using BookHive.Data;
using BookHive.DTOs;
using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web;

namespace BookHive.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DataContext _context;

    public OrderController(IOrderService orderService, ICartService cartService, IEmailService emailService, UserManager<ApplicationUser> userManager,DataContext context)
    {
        _orderService = orderService;
        _cartService = cartService;
        _emailService = emailService;
        _userManager = userManager;
        _context = context;
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
        var user = await _userManager.FindByIdAsync(userId);
        if (!cartItems.Any())
            return RedirectToAction("Index", "Cart");

        var model = new CheckoutViewModel
        {
            CartItems = cartItems,
            TotalAmount = await _cartService.GetCartTotalAsync(userId),
            IsEmailVerified = user.EmailConfirmed
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder()
    {
        var userId = GetUserId();
        var user = await _userManager.FindByIdAsync(userId);

        if (!user.EmailConfirmed)
        {
            TempData["Error"] = "Please verify your email before placing an order.";
            return RedirectToAction("Checkout");
        }

        var cart = await _cartService.GetCartAsync();
        if (!cart.Items.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Checkout");
        }

        try
        {
            await _orderService.CreateOrderAsync(userId);
            await _cartService.ClearCartAsync();
            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("History");
        }
        catch
        {
            TempData["Error"] = "An error occurred while placing your order. Please try again.";
            return RedirectToAction("Checkout");
        }
    }
    [HttpPost]
    public async Task<IActionResult> SendVerificationEmail()
    {
        var userId = GetUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Json(new { success = false, message = "User not found." });
        }

        if (user.EmailConfirmed)
        {
            return Json(new { success = false, message = "Email already verified." });
        }

        try
        {
            var code = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(15);

            var verificationCode = new VerificationCode
            {
                UserId = userId,
                Code = code,
                Expiry = expiry
            };
            _context.VerificationCodes.Add(verificationCode);
            await _context.SaveChangesAsync();

            // Send email
            var email = new DTOs.EmailDto
            {
                To = user.Email,
                Subject = "Verify Your Email - BookHive",
                Body = $@"<p>Dear {user.Name},</p>
                            <p>Your verification code is: <strong>{code}</strong></p>
                            <p>This code expires in 15 minutes.</p>
                            <p>Enter this code on the checkout page to verify your email.</p>
                            <p>Thank you for shopping with BookHive!</p>"
            };

            await _emailService.SendEmailAsync(email);
            return Json(new { success = true, message = "Verification code sent to your email!" });
        }
        catch (ApplicationException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An unexpected error occurred while sending the verification code." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
    {

        if (model == null || string.IsNullOrWhiteSpace(model.Code))
        {
            return Json(new { success = false, message = "Please enter a valid 6-digit code." });
        }

        // Trim the code to handle any whitespace
        model.Code = model.Code.Trim();

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = "Please enter a valid 6-digit code (e.g., 123456)." });
        }

        var userId = GetUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Json(new { success = false, message = "User not found." });
        }

        if (user.EmailConfirmed)
        {
            return Json(new { success = false, message = "Email already verified." });
        }

        try
        {
            var verificationCode = await _context.VerificationCodes
                .FirstOrDefaultAsync(vc => vc.UserId == userId && vc.Code == model.Code && vc.Expiry > DateTime.UtcNow);

            if (verificationCode == null)
            {
                return Json(new { success = false, message = "The verification code is invalid or has expired. Please request a new code." });
            }

            // Mark email as confirmed
            user.EmailConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Json(new { success = false, message = "Failed to verify email due to an internal error. Please contact support." });
            }

            // Remove used code
            _context.VerificationCodes.Remove(verificationCode);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Email verified successfully! You can now place your order." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred during code verification. Please try again or contact support." });
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
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
    {
        var order = await _orderService.GetOrderDetailsAsync(request.OrderId);
        if (order == null)
        {
            return Json(new { success = false, message = "Order not found." });
        }

        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
        {
            return Json(new { success = false, message = "Invalid status." });
        }

        await _orderService.UpdateOrderStatusAsync(request.OrderId, newStatus);
        return Json(new { success = true });
    }
}