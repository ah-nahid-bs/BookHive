using BookHive.Interfaces;
using BookHive.Repositories;

namespace BookHive.Services;
public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }

    public async Task<bool> AddToWishlistAsync(string userId, int bookId)
    {
        return await _wishlistRepository.AddToWishlistAsync(userId, bookId);
    }

    public async Task<bool> RemoveFromWishlistAsync(string userId, int bookId)
    {
        return await _wishlistRepository.RemoveFromWishlistAsync(userId, bookId);
    }

    public async Task<List<int>> GetWishlistBookIdsAsync(string userId)
    {
        return await _wishlistRepository.GetWishlistBookIdsAsync(userId);
    }
}
