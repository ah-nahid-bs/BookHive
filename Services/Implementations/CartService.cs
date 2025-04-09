using BookHive.Data; 

using BookHive.Models; 

using BookHive.Services.Interfaces; 

using Microsoft.EntityFrameworkCore; 

  

namespace BookHive.Services.Implementations 

{ 

    public class CartService : ICartService 

    { 

        private readonly DataContext _context; 

  

        public CartService(DataContext context) 

        { 

            _context = context; 

        } 

  

        public async Task<List<CartItem>> GetUserCartAsync(string userId) 

        { 

            return await _context.CartItems.Include(c => c.Book).Where(c => c.UserId == userId).ToListAsync(); 

        } 

  

        public async Task AddToCartAsync(string userId, int bookId) 

        { 

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId); 

  

            if (cartItem == null) 

            { 

                _context.CartItems.Add(new CartItem { UserId = userId, BookId = bookId, Quantity = 1 }); 

            } 

            else 

            { 

                cartItem.Quantity++; 

            } 

  

            await _context.SaveChangesAsync(); 

        } 

  

        public async Task RemoveFromCartAsync(int cartItemId) 

        { 

            var item = await _context.CartItems.FindAsync(cartItemId); 

            if (item != null) 

            { 

                _context.CartItems.Remove(item); 

                await _context.SaveChangesAsync(); 

            } 

        } 

    } 

} 
