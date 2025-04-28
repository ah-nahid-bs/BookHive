using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;

namespace BookHive.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewViewModel>> GetReviewsByBookIdAsync(int bookId)
    {
        var reviews = await _reviewRepository.GetReviewsByBookIdAsync(bookId);
        return reviews.Select(r => new ReviewViewModel
        {
            Id = r.Id,
            UserId = r.UserId,
            UserName = r.User.Name,
            BookId = r.BookId,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList();
    }

    public async Task<bool> CanUserReviewAsync(string userId, int bookId)
    {
        if (string.IsNullOrEmpty(userId))
            return false;

        var hasPurchased = await _reviewRepository.HasUserPurchasedBookAsync(userId, bookId);
        var existingReview = await _reviewRepository.GetReviewByUserAndBookAsync(userId, bookId);
        return hasPurchased && existingReview == null;
    }

    public async Task AddReviewAsync(ReviewViewModel reviewViewModel)
    {
        var review = new Review
        {
            UserId = reviewViewModel.UserId,
            BookId = reviewViewModel.BookId,
            Rating = reviewViewModel.Rating,
            Comment = reviewViewModel.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.AddReviewAsync(review);
    }
}