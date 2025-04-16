using BookHive.Interfaces;
using BookHive.Models;
using Microsoft.AspNetCore.Identity;

namespace BookHive.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(IAdminRepository adminRepo, UserManager<ApplicationUser> userManager)
    {
        _adminRepo = adminRepo;
        _userManager = userManager;
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync() => await _adminRepo.GetAllUsersAsync();

    public async Task<string?> GetUserRoleAsync(ApplicationUser user) => await _adminRepo.GetUserRoleAsync(user);

    public async Task<bool> ChangeUserRoleAsync(string userId, string newRole, string currentAdminId)
    {
        if (userId == currentAdminId) return false;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRoleAsync(user, newRole);
        return true;
    }
}
