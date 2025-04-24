using BookHive.ViewModels;

namespace BookHive.Interfaces;
public interface ICartService
{
    Task<List<CartItemViewModel>> GetCartItemsAsync(string userId);
    Task AddToCartAsync(string userId, int bookId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(string userId);
    Task<decimal> GetCartTotalAsync(string userId);
    Task<bool> UpdateCartItemAsync(string userId, int cartItemId, int quantity);
    Task<bool> RemoveCartItemAsync(string userId, int cartItemId);

}
