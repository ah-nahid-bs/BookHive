using BookHive.Models;
using BookHive.Services.Interfaces; 

using Microsoft.AspNetCore.Authorization; 

using Microsoft.AspNetCore.Identity; 

using Microsoft.AspNetCore.Mvc; 

  

namespace BookHive.Controllers 

{ 

    [Authorize] 

    public class CartController : Controller 

    { 

        private readonly ICartService _cartService; 

        private readonly UserManager<ApplicationUser> _userManager; 

  

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager) 

        { 

            _cartService = cartService; 

            _userManager = userManager; 

        } 

  

        public async Task<IActionResult> Index() 

        { 

            var user = await _userManager.GetUserAsync(User); 

            var cart = await _cartService.GetUserCartAsync(user.Id); 

            return View(cart); 

        } 

  

        public async Task<IActionResult> Add(int bookId) 

        { 

            var user = await _userManager.GetUserAsync(User); 

            await _cartService.AddToCartAsync(user.Id, bookId); 

            return RedirectToAction("Index"); 

        } 

  

        public async Task<IActionResult> Remove(int id) 

        { 

            await _cartService.RemoveFromCartAsync(id); 

            return RedirectToAction("Index"); 

        } 

    } 

} 

  