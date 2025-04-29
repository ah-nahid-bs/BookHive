using BookHive.Data;
using BookHive.Interfaces;
using BookHive.Models;
using BookHive.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookHive.Services;
public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartService(ICartRepository cartRepository, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CartViewModel> GetCartAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            var cartViewModel = new CartViewModel();

            if (userId != null)
            {
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart != null && cart.CartItems.Any())
                {
                    cartViewModel.Items = cart.CartItems.Select(item => new CartItemViewModel
                    {
                        CartItemId = item.Id,
                        BookId = item.BookId,
                        Title = item.Book.Title,
                        ImageUrl = item.Book.ImageUrl,
                        Price = item.Book.Price,
                        Quantity = item.Quantity
                    }).ToList();
                }
            }
            else
            {
                var sessionCart = GetSessionCart();
                var bookIds = sessionCart.Select(i => i.BookId).ToList();
                var books = await _context.Books
                    .Where(b => bookIds.Contains(b.Id))
                    .ToListAsync();

                cartViewModel.Items = sessionCart.Select(i =>
                {
                    var book = books.FirstOrDefault(b => b.Id == i.BookId);
                    return new CartItemViewModel
                    {
                        CartItemId = 0,
                        BookId = i.BookId,
                        Title = book?.Title ?? "Unknown",
                        ImageUrl = book?.ImageUrl ?? "",
                        Price = book?.Price ?? 0,
                        Quantity = i.Quantity
                    };
                }).ToList();
            }

            cartViewModel.TotalPrice = cartViewModel.Items.Sum(i => i.Total);
            return cartViewModel;
        }

        public async Task AddToCartAsync(int bookId, int quantity)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            if (userId != null)
            {
                await _cartRepository.AddToCartAsync(userId, bookId, quantity);
            }
            else
            {
                var sessionCart = GetSessionCart();
                var item = sessionCart.FirstOrDefault(i => i.BookId == bookId);
                if (item != null)
                {
                    item.Quantity += quantity;
                }
                else
                {
                    sessionCart.Add(new SessionCartItem { BookId = bookId, Quantity = quantity });
                }
                SaveSessionCart(sessionCart);
            }
        }

        public async Task<bool> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            if (userId != null)
            {
                var cartItem = await _cartRepository.GetCartItemByIdAsync(userId, cartItemId);
                if (cartItem == null || quantity <= 0)
                {
                    return false;
                }
                cartItem.Quantity = quantity;
                return await _cartRepository.UpdateCartItemAsync(cartItem);
            }
            else
            {
                var sessionCart = GetSessionCart();
                var item = sessionCart.FirstOrDefault(i => i.BookId == cartItemId); // Using BookId for session
                if (item == null || quantity <= 0)
                {
                    return false;
                }
                item.Quantity = quantity;
                SaveSessionCart(sessionCart);
                return true;
            }
        }

        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            if (userId != null)
            {
                var cartItem = await _cartRepository.GetCartItemByIdAsync(userId, cartItemId);
                if (cartItem == null)
                {
                    return false;
                }
                return await _cartRepository.RemoveCartItemAsync(cartItem);
            }
            else
            {
                var sessionCart = GetSessionCart();
                var item = sessionCart.FirstOrDefault(i => i.BookId == cartItemId); // Using BookId for session
                if (item == null)
                {
                    return false;
                }
                sessionCart.Remove(item);
                SaveSessionCart(sessionCart);
                return true;
            }
        }

        public async Task ClearCartAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            if (userId != null)
            {
                await _cartRepository.ClearCartAsync(userId);
            }
            ClearSessionCart();
        }

        public async Task MergeSessionCartAsync(string userId)
        {
            var sessionCart = GetSessionCart();
            if (sessionCart.Any())
            {
                foreach (var item in sessionCart)
                {
                    await _cartRepository.AddToCartAsync(userId, item.BookId, item.Quantity);
                }
                ClearSessionCart();
            }
        }

        private List<SessionCartItem> GetSessionCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson)
                ? new List<SessionCartItem>()
                : JsonConvert.DeserializeObject<List<SessionCartItem>>(cartJson);
        }

        private void SaveSessionCart(List<SessionCartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
        }

        private void ClearSessionCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove(CartSessionKey);
        }
        public async Task<List<CartItemViewModel>> GetCartItemsAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any()) return new List<CartItemViewModel>();

            return cart.CartItems.Select(item => new CartItemViewModel
            {
                CartItemId = item.Id,
                BookId = item.BookId,
                Title = item.Book.Title,
                ImageUrl = item.Book.ImageUrl,
                Price = item.Book.Price,
                Quantity = item.Quantity
            }).ToList();
        }
        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            var items = await GetCartItemsAsync(userId);
            return items.Sum(i => i.Total);
        }
    }