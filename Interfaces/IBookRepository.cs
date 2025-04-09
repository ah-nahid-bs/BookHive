using BookHive.Models;
namespace BookHive.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
    }
}
