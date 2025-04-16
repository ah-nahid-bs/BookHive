using BookHive.Models;

namespace BookHive.Interfaces;

public interface IAdminService
{
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<string?> GetUserRoleAsync(ApplicationUser user);
    Task<bool> ChangeUserRoleAsync(string userId, string newRole, string currentAdminId);
}