
using BookHive.Models;

namespace BookHive.Services.Interfaces 

{ 

    public interface IBookService 

    { 

        Task<Book?> GetBookByIdAsync(int id); 

        Task<List<Book>> GetBooksAsync(); 

    } 

 }