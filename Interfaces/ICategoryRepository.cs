namespace BookHive.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<string>> GetAllCategoryTitlesAsync();
    }
}
