using BookHive.Data;
using BookHive.Models;
using BookHive.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Services;
public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<List<UserInterest>> GetUserCategoryInterestsAsync(string userId)
    {
        var categoryInterests = await _context.Orders
            .Where(o => o.UserId == userId)
            .Join(_context.OrderItems,
                o => o.Id,
                oi => oi.OrderId,
                (o, oi) => new { OrderItem = oi })
            .Join(_context.Books,
                oi => oi.OrderItem.BookId,
                b => b.Id,
                (oi, b) => new { Book = b })
            .Join(_context.Categories,
                b => b.Book.CategoryId,
                c => c.Id,
                (b, c) => new UserInterest
                {
                    UserId = userId,
                    InterestType = "Category",
                    CategoryId = c.Id,
                    Category = c,
                })
            .GroupBy(ui => ui.CategoryId)
            .Select(g => g.First())
            .Take(3) // Limit to 2-3 categories
            .ToListAsync();

        return categoryInterests;
    }

    public async Task<List<UserInterest>> GetUserAuthorInterestsAsync(string userId)
    {
        var authorInterests = await _context.Orders
            .Where(o => o.UserId == userId)
            .Join(_context.OrderItems,
                o => o.Id,
                oi => oi.OrderId,
                (o, oi) => new { OrderItem = oi })
            .Join(_context.Books,
                oi => oi.OrderItem.BookId,
                b => b.Id,
                (oi, b) => new UserInterest
                {
                    UserId = userId,
                    InterestType = "Author",
                    AuthorName = b.Author,
                })
            .GroupBy(ui => ui.AuthorName)
            .Select(g => g.First())
            .Take(3) // Limit to 2-3 authors
            .ToListAsync();

        return authorInterests;
    }
}