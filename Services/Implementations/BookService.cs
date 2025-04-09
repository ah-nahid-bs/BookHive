using BookHive.Data; 

using BookHive.Models; 

using BookHive.Services.Interfaces; 

using Microsoft.EntityFrameworkCore; 

  

namespace BookHive.Services.Implementations 

{ 

    public class BookService : IBookService

    { 

        private readonly DataContext _context; 

  

        public BookService(DataContext context) 

        { 

            _context = context; 

        } 

  

        public async Task<Book?> GetBookByIdAsync(int id) 

        { 

            return await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id); 

        } 

  

        public async Task<List<Book>> GetBooksAsync() 

        { 

            return await _context.Books.Include(b => b.Category).ToListAsync(); 

        } 

    } 

} 