using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminService _adminService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOrderService _orderService;

    public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        _adminService = adminService;
        _userManager = userManager;
        _orderService = orderService;
    }

    public async Task<IActionResult> Users()
    {
        var users = await _adminService.GetAllUsersAsync();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CurrentRole = roles.FirstOrDefault() ?? "No Role"
            };
            userViewModels.Add(userViewModel);
        }

        return View(userViewModels);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(string userId, string newRole)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Unauthorized();

        var result = await _adminService.ChangeUserRoleAsync(userId, newRole, currentUser.Id);
        return RedirectToAction("Users");
    }

    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> OrderDetails(int id)
    {
        var order = await _orderService.GetOrderDetailsAsync(id);
        if (order == null)
            return NotFound();

        return View(order);
    }
}