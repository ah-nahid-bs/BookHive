namespace BookHive.ViewModels;

public class OrderViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemViewModel> Items { get; set; }

    public string InferredStatus =>
        Items != null && Items.Any() && Items.All(i => i.Status == Items[0].Status)
            ? Items[0].Status.ToString()
            : "Pending";
}
