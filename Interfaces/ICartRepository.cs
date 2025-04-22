using BookHive.Models;

namespace BookHive.Interfaces;
public interface ICartRepository
{
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task AddToCartAsync(string userId, int bookId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(string userId);
    Task SaveAsync();
}

