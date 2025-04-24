namespace BookHive.Repositories;
public interface IWishlistRepository
{
    Task<bool> AddToWishlistAsync(string userId, int bookId);
    Task<bool> RemoveFromWishlistAsync(string userId, int bookId);
    Task<List<int>> GetWishlistBookIdsAsync(string userId);
    Task<bool> IsBookInWishlistAsync(string userId, int bookId);
}