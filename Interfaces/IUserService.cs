using BookHive.Models;

namespace BookHive.Interfaces;
public interface IUserService
{
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<List<UserInterest>> GetUserCategoryInterestsAsync(string userId);
    Task<List<UserInterest>> GetUserAuthorInterestsAsync(string userId);
}