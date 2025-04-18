using BookHive.ViewModel;

namespace BookHive.Interfaces;
public interface ICartService
{
    Task<List<CartItemViewModel>> GetCartItemsAsync(string userId);
    Task AddToCartAsync(string userId, int bookId, int quantity);
    Task RemoveFromCartAsync(int cartItemId);
    Task ClearCartAsync(string userId);
    Task<decimal> GetCartTotalAsync(string userId);

}
