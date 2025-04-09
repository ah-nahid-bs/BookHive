using BookHive.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookHive.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;

        public HomeController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categoryTitles = await _categoryService.GetAllCategoryTitlesAsync();
            ViewBag.Categories = categoryTitles;
            return View();
        }
    }
}
