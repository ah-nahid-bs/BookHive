namespace BookHive.ViewModels;
public class DiscountedBookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public string ImageUrl { get; set; }
}
