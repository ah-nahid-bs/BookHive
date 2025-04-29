using BookHive.Models;

namespace BookHive.Interfaces;
public interface IReviewRepository
{
    Task<List<Review>> GetReviewsByBookIdAsync(int bookId);
    Task<Review> GetReviewByUserAndBookAsync(string userId, int bookId);
    Task AddReviewAsync(Review review);
    Task<bool> HasUserPurchasedBookAsync(string userId, int bookId);
}