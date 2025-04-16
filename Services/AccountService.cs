using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BookHive.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;


    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        if (model.Password != model.ConfirmPassword)
            return false;

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name
        };   

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            return true;
        }
        return result.Succeeded;
    }
    public async Task<bool> LoginAsync(LoginViewModel model)
    {
        // Try to find user by Email or Username
        var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
        }

        if (user == null)
        {
            return false;
        }

        // Attempt to sign in
        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            model.Password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
        {
            Console.WriteLine("Login succeeded.");
            return true;
        }

        return false;
    }

}
