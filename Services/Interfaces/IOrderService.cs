
using BookHive.Models;

namespace BookHive.Services.Interfaces 

{ 

    public interface IOrderService 

    { 

        Task<Order> PlaceOrderAsync(string userId); 

    } 

} 
