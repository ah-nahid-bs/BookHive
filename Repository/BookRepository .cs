using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories
{
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
    }
}
