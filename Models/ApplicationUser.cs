using Microsoft.AspNetCore.Identity;

namespace BookHive.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<Wishlist> WishlistItems { get; set; }
}
