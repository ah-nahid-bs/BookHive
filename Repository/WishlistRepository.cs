using BookHive.Data;
using BookHive.Models;
using Microsoft.EntityFrameworkCore;
namespace BookHive.Repositories;
public class WishlistRepository : IWishlistRepository
{
    private readonly DataContext _context;

    public WishlistRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddToWishlistAsync(string userId, int bookId)
    {
        if (!await _context.Books.AnyAsync(b => b.Id == bookId))
        {
            return false;
        }

        if (await IsBookInWishlistAsync(userId, bookId))
        {
            return false;
        }

        var wishlistItem = new Wishlist
        {
            UserId = userId,
            BookId = bookId
        };

        _context.Wishlists.Add(wishlistItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromWishlistAsync(string userId, int bookId)
    {
        var wishlistItem = await _context.Wishlists
            .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);

        if (wishlistItem == null)
        {
            return false;
        }

        _context.Wishlists.Remove(wishlistItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<int>> GetWishlistBookIdsAsync(string userId)
    {
        return await _context.Wishlists
            .Where(w => w.UserId == userId)
            .Select(w => w.BookId)
            .ToListAsync();
    }

    public async Task<bool> IsBookInWishlistAsync(string userId, int bookId)
    {
        return await _context.Wishlists
            .AnyAsync(w => w.UserId == userId && w.BookId == bookId);
    }
}
