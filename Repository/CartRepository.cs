using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories;
public class CartRepository : ICartRepository
{
    private readonly DataContext _context;

    public CartRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Book)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddToCartAsync(string userId, int bookId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                BookId = bookId,
                Quantity = quantity
            });
        }

        await SaveAsync();
    }

    public async Task RemoveFromCartAsync(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await SaveAsync();
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart != null)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
            await SaveAsync();
        }
    }
    public async Task<CartItem> GetCartItemByIdAsync(string userId, int cartItemId)
    {
        return await _context.CartItems
            .Include(c => c.Cart)
            .FirstOrDefaultAsync(c => c.Id == cartItemId && c.Cart.UserId == userId);
    }
    public async Task<bool> UpdateCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }


    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

}
