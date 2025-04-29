using BookHive.ViewModels;

namespace BookHive.Interfaces;
public interface IUserProfileService
{
    Task<UserProfileViewModel> GetUserProfileAsync(string userId);
    Task UpdateUserProfileAsync(UserProfileViewModel model);
}