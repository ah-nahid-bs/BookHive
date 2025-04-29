using Microsoft.AspNetCore.Identity;

namespace BookHive.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string? Address { get; set; }
     public ICollection<Wishlist> WishlistItems { get; set; } = new List<Wishlist>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}
