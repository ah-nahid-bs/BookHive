using BookHive.Models;
using BookHive.ViewModels;
namespace BookHive.Interfaces;
public interface IBookRepository
{
    Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
    Task<IEnumerable<Book>> GetNewArrivals();
    Task<IEnumerable<Book>> GetBestSellersAsync(int minimumSold);
    Task<IEnumerable<Book>> GetTrendingBooksThisMonthAsync(int topCount);
    Task<IEnumerable<DiscountedBookViewModel>> GetDiscountedBooksAsync(decimal discountPercent);

    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(string categoryName);
    Task<Book?> GetBookByIdAsync(int id);
    Task<(IEnumerable<Book>, int)> SearchBooksAsync(string query, int page, int pageSize);

}
