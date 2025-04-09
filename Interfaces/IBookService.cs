using BookHive.Models;

namespace BookHive.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
    }
}
