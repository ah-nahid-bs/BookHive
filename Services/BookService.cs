using BookHive.Interfaces;
using BookHive.Models;

namespace BookHive.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10)
        {
            return await _bookRepository.GetFeaturedBooksAsync(count);
        }
        
        public async Task<IEnumerable<Book>> GetNewArrivals()
        {
            return await _bookRepository.GetNewArrivals();
        }
    }
}
