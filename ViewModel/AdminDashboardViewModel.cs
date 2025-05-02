using BookHive.Models;
namespace BookHive.ViewModels;
public class AdminDashboardViewModel
{
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int MonthlyOrderCount { get; set; }
    public List<ApplicationUser> Users { get; set; }
}