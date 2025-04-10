namespace BookHive.Models;
public class CartItem
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int Quantity { get; set; }
}
