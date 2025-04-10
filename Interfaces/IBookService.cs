using BookHive.Models;
using BookHive.ViewModels;

namespace BookHive.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
        Task<IEnumerable<Book>> GetNewArrivals();
        Task<IEnumerable<Book>> GetBestSellersAsync();
        Task<IEnumerable<Book>> GetTrendingBooksThisMonthAsync();
        Task<IEnumerable<DiscountedBookViewModel>> GetDiscountedBooksAsync();

    }
}
