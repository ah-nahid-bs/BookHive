using BookHive.Models;

namespace BookHive.ViewModels;
public class OrderItemViewModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total => Price * Quantity;
    public OrderStatus Status { get; set; }
}