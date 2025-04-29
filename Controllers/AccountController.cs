using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookHive.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserProfileService _userProfileService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICartService _cartService;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager, IUserProfileService userProfileService, ICartService cartService)
        {
            _accountService = accountService;
            _signInManager = signInManager;
            _userProfileService = userProfileService;
            _cartService = cartService;
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
            if (!ModelState.IsValid)
            {
                System.Console.WriteLine("Model state is invalid.");
                return View(model);
            }

            var result = await _accountService.RegisterAsync(model);
            System.Console.WriteLine($"Registration result: {result}");
            if (result)
            {
                System.Console.WriteLine("Registration successful.");
                return RedirectToAction("Index", "Home");
            }
            System.Console.WriteLine("Registration failed.");
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
                System.Console.WriteLine("Login successful.");
                var user = await _signInManager.UserManager.FindByEmailAsync(model.UsernameOrEmail);
                if (user != null)
                {
                    await _cartService.MergeSessionCartAsync(user.Id);
                }
                return RedirectToAction("Index", "Home");
            }

            System.Console.WriteLine("Login failed.");
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
}