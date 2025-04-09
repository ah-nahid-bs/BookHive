using BookHive.Models;

namespace BookHive.Services.Interfaces 

{ 

    public interface ICartService 

    { 

        Task<List<CartItem>> GetUserCartAsync(string userId); 

        Task AddToCartAsync(string userId, int bookId); 

        Task RemoveFromCartAsync(int cartItemId); 

    } 

} 
