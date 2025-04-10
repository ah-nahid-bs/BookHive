using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;

namespace BookHive.Services;
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
    public async Task<IEnumerable<Book>> GetBestSellersAsync()
    {
        int minimumSold = 10;//this will be updated later on to reciee form admin
        return await _bookRepository.GetBestSellersAsync(minimumSold);
    }
    public async Task<IEnumerable<Book>> GetTrendingBooksThisMonthAsync()
    {
        int topCount = 5; //this will be updated later on to reciee form admin if needed
        return await _bookRepository.GetTrendingBooksThisMonthAsync(topCount);
    }
    public async Task<IEnumerable<DiscountedBookViewModel>> GetDiscountedBooksAsync()
    {
        decimal discountPercent = 15m; // this will be updated later on to reciee form admin
        return await _bookRepository.GetDiscountedBooksAsync(discountPercent);
    }

}

