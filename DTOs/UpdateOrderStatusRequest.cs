namespace BookHive.DTOs;
public class UpdateOrderStatusRequest
{
    public int OrderId { get; set; }
    public string Status { get; set; }
}