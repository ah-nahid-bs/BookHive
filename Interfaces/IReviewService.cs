using BookHive.ViewModels;

namespace BookHive.Interfaces;
public interface IReviewService
{
    Task<List<ReviewViewModel>> GetReviewsByBookIdAsync(int bookId);
    Task<bool> CanUserReviewAsync(string userId, int bookId);
    Task AddReviewAsync(ReviewViewModel review);
}