namespace  BookHive.Interfaces;
public interface IWishlistService
{
    Task<bool> AddToWishlistAsync(string userId, int bookId);
    Task<bool> RemoveFromWishlistAsync(string userId, int bookId);
    Task<List<int>> GetWishlistBookIdsAsync(string userId);
}