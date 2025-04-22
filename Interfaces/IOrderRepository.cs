using BookHive.Models;

namespace BookHive.Interfaces;
public interface IOrderRepository
{
    Task CreateOrderAsync(Order order);
    Task<List<Order>> GetUserOrdersAsync(string userId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(int orderId);
}