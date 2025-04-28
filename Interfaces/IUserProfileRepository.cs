using BookHive.Models;

namespace BookHive.Interfaces;
public interface IUserProfileRepository
{
    Task<ApplicationUser> GetUserByIdAsync(string userId);
    Task UpdateUserAsync(ApplicationUser user);
}