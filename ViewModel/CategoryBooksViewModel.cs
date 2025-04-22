namespace BookHive.ViewModels;

public class CategoryBooksViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public List<BookViewModel> Books { get; set; } = new();
}
