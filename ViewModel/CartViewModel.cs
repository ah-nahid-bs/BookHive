namespace BookHive.ViewModels;
public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    public decimal TotalPrice { get; set; }
}