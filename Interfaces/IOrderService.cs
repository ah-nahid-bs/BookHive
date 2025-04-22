using BookHive.ViewModels;

namespace BookHive.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(string userId);
    Task<List<OrderViewModel>> GetUserOrdersAsync(string userId);
    Task<List<OrderViewModel>> GetAllOrdersAsync(); // For admin
    Task<OrderViewModel> GetOrderDetailsAsync(int orderId);
}