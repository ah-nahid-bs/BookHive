using BookHive.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;
        public HomeController(ICategoryService categoryService, IBookService bookService)
        {
            _categoryService = categoryService;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var categoryTitles = await _categoryService.GetAllCategoryTitlesAsync();
            var featuredItems = await _bookService.GetFeaturedBooksAsync();

            ViewBag.Categories = categoryTitles;
            ViewBag.FeaturedItems = featuredItems;
            return View();
        }
    }
}
