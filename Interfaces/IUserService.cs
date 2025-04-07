using BookHive.Models;
namespace BookHive.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameOrEmail(string input);
        Task CreateUser(User user);
    }
}