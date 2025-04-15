using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BookHive.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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
        return result.Succeeded;
    }
}
