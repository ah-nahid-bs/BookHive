namespace BookHive.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<string>> GetAllCategoryTitlesAsync();
    }
}
