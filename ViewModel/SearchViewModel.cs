namespace BookHive.ViewModels;
public class SearchViewModel
{
    public string Query { get; set; }
    public List<BookViewModel> Results { get; set; } = new List<BookViewModel>();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}