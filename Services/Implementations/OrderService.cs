using BookHive.Data; 

using BookHive.Models; 

using BookHive.Services.Interfaces; 

using Microsoft.EntityFrameworkCore; 

  

namespace BookHive.Services.Implementations 

{ 

    public class OrderService : IOrderService 

    { 

        private readonly DataContext _context; 

  

        public OrderService(DataContext context) 

        { 

            _context = context; 

        } 

  

        public async Task<Order> PlaceOrderAsync(string userId) 

        { 

            var cartItems = await _context.CartItems.Include(c => c.Book).Where(c => c.UserId == userId).ToListAsync(); 

  

            if (!cartItems.Any()) throw new Exception("Cart is empty"); 

  

            var order = new Order { UserId = userId }; 

  

            foreach (var item in cartItems) 

            { 

                order.Items.Add(new OrderItem 

                { 

                    BookId = item.BookId, 

                    Quantity = item.Quantity 

                }); 

            } 

  

            _context.Orders.Add(order); 

            _context.CartItems.RemoveRange(cartItems); 

            await _context.SaveChangesAsync(); 

  

            return order; 

        } 

    } 

} 
