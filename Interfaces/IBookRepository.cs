using BookHive.Models;
using BookHive.ViewModels;
namespace BookHive.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
        Task<IEnumerable<Book>> GetNewArrivals();
        Task<IEnumerable<Book>> GetBestSellersAsync(int minimumSold);
        Task<IEnumerable<Book>> GetTrendingBooksThisMonthAsync(int topCount);
        Task<IEnumerable<DiscountedBookViewModel>> GetDiscountedBooksAsync(decimal discountPercent);

    }
}
