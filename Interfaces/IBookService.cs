using BookHive.Models;
using BookHive.ViewModels;

namespace BookHive.Interfaces;
public interface IBookService
{
    Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
    Task<IEnumerable<Book>> GetNewArrivals();
    Task<IEnumerable<Book>> GetBestSellersAsync();
    Task<IEnumerable<Book>> GetTrendingBooksThisMonthAsync();
    Task<IEnumerable<DiscountedBookViewModel>> GetDiscountedBooksAsync();
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(string categoryName);

}
