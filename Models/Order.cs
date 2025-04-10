namespace BookHive.Models;
public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
