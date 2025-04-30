

using BookHive.Models;

namespace BookHive.Extensions;
public static class OrderStatusExtensions
{
    public static IEnumerable<OrderStatus> GetValidNextStatuses(OrderStatus currentStatus)
    {
        switch (currentStatus)
        {
            case OrderStatus.Pending:
                return new[] { OrderStatus.Processing, OrderStatus.Cancelled };
            case OrderStatus.Processing:
                return new[] { OrderStatus.Shipped, OrderStatus.Cancelled };
            case OrderStatus.Shipped:
                return new[] { OrderStatus.Delivered, OrderStatus.Cancelled };
            case OrderStatus.Delivered:
                return Array.Empty<OrderStatus>();
            case OrderStatus.Cancelled:
                return Array.Empty<OrderStatus>();
            default:
                return Array.Empty<OrderStatus>();
        }
    }
}