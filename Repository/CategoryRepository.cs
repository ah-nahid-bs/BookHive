using BookHive.Data;
using BookHive.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetAllCategoryTitlesAsync()
        {
            return await _context.Categories
                                 .Select(c => c.Name)
                                 .ToListAsync();
        }
    }
}
