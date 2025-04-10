using BookHive.Interfaces;

namespace BookHive.Services;
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<string>> GetAllCategoryTitlesAsync()
    {
        return await _categoryRepository.GetAllCategoryTitlesAsync();
    }
}
