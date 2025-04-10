using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories;
public class BookRepository : IBookRepository
{
    private readonly DataContext _context;

    public BookRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10)
    {
        return await _context.Books
                                .Where(b => b.IsFeatured)
                                .Take(count)
                                .ToListAsync();
    }
    public async Task<IEnumerable<Book>> GetNewArrivals()
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        return await _context.Books
            .Where(b => b.PublishDate.Year == currentDate.Year && b.PublishDate.Month == currentDate.Month)
            .OrderByDescending(b => b.PublishDate)
            .Take(10)
            .ToListAsync();
    }
    public async Task<IEnumerable<Book>> GetBestSellersAsync(int minimumSold)
    {
        return await _context.Books
            .Where(b => b.TotalSold >= minimumSold)
            .OrderByDescending(b => b.TotalSold)
            .ToListAsync();
    }
    public async Task<IEnumerable<Book>> GetTrendingBooksThisMonthAsync(int topCount)
    {
        var now = DateTime.Now;

        var books = await _context.OrderItems
            .Where(oi => oi.Order.OrderDate.Month == now.Month && oi.Order.OrderDate.Year == now.Year)
            .GroupBy(oi => oi.Book)
            .Select(g => new {
                Book = g.Key,
                TotalSold = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(g => g.TotalSold)
            .Take(topCount)
            .Select(g => g.Book)
            .ToListAsync();

        return books;
    }
    public async Task<IEnumerable<DiscountedBookViewModel>> GetDiscountedBooksAsync(decimal discountPercent)
    {
        return await _context.Books
            .Where(b => b.IsDiscounted)
            .Select(b => new DiscountedBookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                OriginalPrice = b.Price,
                DiscountedPrice = Math.Round(b.Price * (1 - discountPercent / 100), 2),
                ImageUrl = b.ImageUrl
            })
            .ToListAsync();
    }

}
