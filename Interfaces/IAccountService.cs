using BookHive.ViewModels;

namespace BookHive.Interfaces;
public interface IAccountService
{
    Task<bool> RegisterAsync(RegisterViewModel model);
    Task<bool> LoginAsync(LoginViewModel model);

}
