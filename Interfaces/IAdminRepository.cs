using BookHive.Models;

namespace BookHive.Interfaces;

public interface IAdminRepository
{
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<string?> GetUserRoleAsync(ApplicationUser user);
    Task<bool> ChangeUserRoleAsync(ApplicationUser user, string newRole);
}
