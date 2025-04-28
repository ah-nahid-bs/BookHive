using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> GetReviewsByBookIdAsync(int bookId)
    {
        return await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review> GetReviewByUserAndBookAsync(string userId, int bookId)
    {
        return await _context.Reviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);
    }

    public async Task AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasUserPurchasedBookAsync(string userId, int bookId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .AnyAsync(oi => oi.BookId == bookId && oi.Order.UserId == userId && oi.Status == OrderStatus.Delivered);
    }
}