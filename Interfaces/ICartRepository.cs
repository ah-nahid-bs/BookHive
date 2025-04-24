using BookHive.Models;

namespace BookHive.Interfaces;
public interface ICartRepository
{
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task AddToCartAsync(string userId, int bookId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(string userId);
    Task SaveAsync();
    Task<CartItem> GetCartItemByIdAsync(string userId, int cartItemId);
    Task<bool> UpdateCartItemAsync(CartItem cartItem);
    Task<bool> RemoveCartItemAsync(CartItem cartItem);
}

