using BookHive.Interfaces;
using BookHive.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync() => _userManager.Users.ToList();

    public async Task<string?> GetUserRoleAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.FirstOrDefault();
    }

    public async Task<bool> ChangeUserRoleAsync(ApplicationUser user, string newRole)
    {
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        var result = await _userManager.AddToRoleAsync(user, newRole);
        return result.Succeeded;
    }
}
