using BookHive.Interfaces;
using BookHive.ViewModels;

namespace BookHive.Services;
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<List<CartItemViewModel>> GetCartItemsAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null || !cart.CartItems.Any()) return new List<CartItemViewModel>();

        return cart.CartItems.Select(item => new CartItemViewModel
        {
            CartItemId = item.Id,
            BookId = item.BookId,
            Title = item.Book.Title,
            ImageUrl = item.Book.ImageUrl,
            Price = item.Book.Price,
            Quantity = item.Quantity
        }).ToList();
    }

    public async Task AddToCartAsync(string userId, int bookId, int quantity)
    {
        await _cartRepository.AddToCartAsync(userId, bookId, quantity);
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        await _cartRepository.RemoveFromCartAsync(cartItemId);
    }

    public async Task ClearCartAsync(string userId)
    {
        await _cartRepository.ClearCartAsync(userId);
    }

    public async Task<decimal> GetCartTotalAsync(string userId)
    {
        var items = await GetCartItemsAsync(userId);
        return items.Sum(i => i.Total);
    }
    public async Task<bool> UpdateCartItemAsync(string userId, int cartItemId, int quantity)
    {
        if (string.IsNullOrEmpty(userId) || cartItemId <= 0 || quantity <= 0)
        {
            return false;
        }

        var cartItem = await _cartRepository.GetCartItemByIdAsync(userId, cartItemId);
        if (cartItem == null)
        {
            return false;
        }

        cartItem.Quantity = quantity;
        return await _cartRepository.UpdateCartItemAsync(cartItem);
    }

    public async Task<bool> RemoveCartItemAsync(string userId, int cartItemId)
    {
        if (string.IsNullOrEmpty(userId) || cartItemId <= 0)
        {
            return false;
        }

        var cartItem = await _cartRepository.GetCartItemByIdAsync(userId, cartItemId);
        if (cartItem == null)
        {
            return false;
        }

        return await _cartRepository.RemoveCartItemAsync(cartItem);
    }

}