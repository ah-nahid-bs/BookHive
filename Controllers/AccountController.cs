using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IUserProfileService _userProfileService;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager,IUserProfileService userProfileService)
    {
        _accountService = accountService;
        _signInManager = signInManager;
        _userProfileService = userProfileService;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(RegisterViewModel model)
    {
        if (!ModelState.IsValid){
            Console.WriteLine("Model state is invalid.");
            return View(model);
        }

        var result = await _accountService.RegisterAsync(model);
        Console.WriteLine($"Registration result: {result}");
        if (result)
        {
            Console.WriteLine("Registration successful.");
            return RedirectToAction("Index", "Home");
        }
        Console.WriteLine("Registration failed.");
        ModelState.AddModelError("", "Registration failed.");
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _accountService.LoginAsync(model);
        if (result)
        {
            Console.WriteLine("Login successful.");
            return RedirectToAction("Index", "Home");
        }

        Console.WriteLine("Login failed. with error");
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
      [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var model = await _userProfileService.GetUserProfileAsync(userId);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var model = await _userProfileService.GetUserProfileAsync(userId);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _userProfileService.UpdateUserProfileAsync(model);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }
}
