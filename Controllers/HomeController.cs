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
            var newArrivals = await _bookService.GetNewArrivals();
            var bestSellers = await _bookService.GetBestSellersAsync();
            var trendingBooks = await _bookService.GetTrendingBooksThisMonthAsync();
            var discountedBooks = await _bookService.GetDiscountedBooksAsync();

            ViewBag.Categories = categoryTitles;
            ViewBag.FeaturedItems = featuredItems;
            ViewBag.NewArrivals = newArrivals;
            ViewBag.BestSellers = bestSellers;
            ViewBag.TrendingBooks = trendingBooks;
            ViewBag.DiscountedBooks = discountedBooks;

            return View();
        }
    }
}
