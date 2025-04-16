namespace BookHive.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string CurrentRole { get; set; } = string.Empty;

    public string NewRole { get; set; } = string.Empty;
}
