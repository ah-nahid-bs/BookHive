using BookHive.Models;
using BookHive.ViewModels;

namespace BookHive.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(string userId);
    Task<List<OrderViewModel>> GetUserOrdersAsync(string userId);
    Task<List<OrderViewModel>> GetAllOrdersAsync(); // For admin
    Task<OrderViewModel> GetOrderDetailsAsync(int orderId);
    Task UpdateOrderStatusAsync(int orderId, OrderStatus status);

    Task<decimal> GetTotalSalesRevenueAsync();
    Task<decimal> GetMonthlySalesRevenueAsync(int year, int month);
    Task<int> GetMonthlyOrderCountAsync(int year, int month);
}