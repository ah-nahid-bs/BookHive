using BookHive.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Controllers
{
    public class BooksController : Controller
    {
        private readonly DataContext _context;
        public BooksController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.Include(b => b.Category).ToListAsync();
            return View(books);
        }
    }
}
