using BookHive.Data;
using BookHive.Extensions;
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
            .Where(oi => oi.Order != null &&
                        oi.Order.OrderDate.Month == now.Month &&
                        oi.Order.OrderDate.Year == now.Year)
            .GroupBy(oi => oi.Book)
            .Select(g => new
            {
                Book = g.Key,
                TotalSold = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(g => g.TotalSold)
            .Take(topCount)
            .Select(g => g.Book!)
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

    public async Task<List<Book>> GetAllAsync() => await _context.Books.Include(b => b.Category).ToListAsync();

    public async Task<Book?> GetByIdAsync(int id) => await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(string categoryName)
    {
        return await _context.Books
            .Include(b => b.Category)
            .Where(b => b.Category != null && b.Category.Name == categoryName)
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<(IEnumerable<Book>, int)> SearchBooksAsync(string query, int page, int pageSize)
    {
        IQueryable<Book> queryable = _context.Books.Include(b => b.Category);

        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.ToLower();
            queryable = queryable.Where(b =>
                b.Title.ToLower().Contains(query) ||
                b.Author.ToLower().Contains(query) ||
                b.Category.Name.ToLower().Contains(query));
        }

        var totalCount = await queryable.CountAsync();

        var books = await queryable
            .OrderBy(b => b.Title)
            .ApplyPagination(page, pageSize)
            .ToListAsync();

        return (books, totalCount);
    }
}