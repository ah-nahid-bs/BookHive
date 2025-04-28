
using BookHive.Interfaces;
using BookHive.ViewModels;

namespace BookHive.Services;
public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;

    public UserProfileService(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UserProfileViewModel> GetUserProfileAsync(string userId)
    {
        var user = await _userProfileRepository.GetUserByIdAsync(userId);
        if (user == null)
            return null;

        return new UserProfileViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address
        };
    }

    public async Task UpdateUserProfileAsync(UserProfileViewModel model)
    {
        var user = await _userProfileRepository.GetUserByIdAsync(model.Id);
        if (user == null)
            return;

        user.Name = model.Name;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;

        await _userProfileRepository.UpdateUserAsync(user);
    }
}