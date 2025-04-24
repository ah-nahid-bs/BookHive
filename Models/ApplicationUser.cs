using Microsoft.AspNetCore.Identity;

namespace BookHive.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public ICollection<Wishlist> WishlistItems { get; set; }
}
