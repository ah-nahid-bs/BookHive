using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;

namespace BookHive.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartService _cartService;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, ICartService cartService)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _cartService = cartService;
    }

    public async Task CreateOrderAsync(string userId)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            throw new InvalidOperationException("Cart is empty.");

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalAmount = cart.CartItems.Sum(ci => ci.Book.Price * ci.Quantity),
            Items = cart.CartItems.Select(ci => new OrderItem
            {
                BookId = ci.BookId,
                Quantity = ci.Quantity,
                Price = ci.Book.Price 
            }).ToList()
        };

        foreach (var item in cart.CartItems)
        {
            var book = item.Book;
            book.TotalSold += item.Quantity;
        }

        await _orderRepository.CreateOrderAsync(order);
        await _cartRepository.ClearCartAsync(userId); 
    }

    public async Task<List<OrderViewModel>> GetUserOrdersAsync(string userId)
    {
        var orders = await _orderRepository.GetUserOrdersAsync(userId);
        return orders.Select(o => new OrderViewModel
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Items = o.Items.Select(oi => new OrderItemViewModel
            {
                Id = oi.Id,
                BookId = oi.BookId,
                Title = oi.Book.Title,
                ImageUrl = oi.Book.ImageUrl,
                Price = oi.Price,
                Quantity = oi.Quantity
            }).ToList()
        }).ToList();
    }

    public async Task<List<OrderViewModel>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        return orders.Select(o => new OrderViewModel
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Items = o.Items.Select(oi => new OrderItemViewModel
            {
                Id = oi.Id,
                BookId = oi.BookId,
                Title = oi.Book.Title,
                ImageUrl = oi.Book.ImageUrl,
                Price = oi.Price,
                Quantity = oi.Quantity
            }).ToList()
        }).ToList();
    }

    public async Task<OrderViewModel> GetOrderDetailsAsync(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            return null;

        return new OrderViewModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(oi => new OrderItemViewModel
            {
                Id = oi.Id,
                BookId = oi.BookId,
                Title = oi.Book.Title,
                ImageUrl = oi.Book.ImageUrl,
                Price = oi.Price,
                Quantity = oi.Quantity
            }).ToList()
        };
    }
}