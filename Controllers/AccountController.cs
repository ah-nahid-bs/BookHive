using BookHive.Interfaces;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult SignUp()
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
}
