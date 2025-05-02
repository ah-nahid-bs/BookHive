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
    private readonly IUserService _userService;

    public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, IOrderService orderService, IUserService userService)
    {
        _adminService = adminService;
        _userManager = userManager;
        _orderService = orderService;
        _userService = userService;
    }
    public async Task<IActionResult> Reports()
    {
        var totalRevenue = await _orderService.GetTotalSalesRevenueAsync();
        var currentMonth = DateTime.Now;
        var monthlyRevenue = await _orderService.GetMonthlySalesRevenueAsync(currentMonth.Year, currentMonth.Month);
        var monthlyOrders = await _orderService.GetMonthlyOrderCountAsync(currentMonth.Year, currentMonth.Month);
        var users = await _userService.GetAllUsersAsync();

        var viewModel = new AdminDashboardViewModel
        {
            TotalRevenue = totalRevenue,
            MonthlyRevenue = monthlyRevenue,
            MonthlyOrderCount = monthlyOrders,
            Users = users
        };

        return View(viewModel);
    }

    public async Task<IActionResult> UserCategoryInterests(string userId)
    {
        var interests = await _userService.GetUserCategoryInterestsAsync(userId);
        var user = await _userService.GetAllUsersAsync();
        var userName = user.FirstOrDefault(u => u.Id == userId)?.UserName ?? "Unknown";

        var viewModel = new UserInterestsViewModel
        {
            UserId = userId,
            UserName = userName,
            Interests = interests
        };

        return View(viewModel);
    }

    public async Task<IActionResult> UserAuthorInterests(string userId)
    {
        var interests = await _userService.GetUserAuthorInterestsAsync(userId);
        var user = await _userService.GetAllUsersAsync();
        var userName = user.FirstOrDefault(u => u.Id == userId)?.UserName ?? "Unknown";

        var viewModel = new UserInterestsViewModel
        {
            UserId = userId,
            UserName = userName,
            Interests = interests
        };

        return View(viewModel);
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