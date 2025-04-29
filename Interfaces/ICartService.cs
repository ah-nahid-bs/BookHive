using BookHive.ViewModels;

namespace BookHive.Interfaces;
public interface ICartService
{
    Task<CartViewModel> GetCartAsync();
    Task AddToCartAsync(int bookId, int quantity);
    Task<bool> UpdateCartItemAsync(int cartItemId, int quantity);
    Task<bool> RemoveCartItemAsync(int cartItemId);
    Task ClearCartAsync();
    Task MergeSessionCartAsync(string userId);
    Task<decimal> GetCartTotalAsync(string userId);
    Task<List<CartItemViewModel>> GetCartItemsAsync(string userId);
}
