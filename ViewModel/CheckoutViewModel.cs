
namespace BookHive.ViewModels;
public class CheckoutViewModel
{
    public decimal TotalAmount { get; set; }
    public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
}