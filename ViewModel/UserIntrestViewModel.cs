using BookHive.Models;
namespace BookHive.ViewModels;
public class UserInterestsViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<UserInterest> Interests { get; set; }
}