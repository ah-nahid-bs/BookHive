using BookHive.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookHive.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);

                entity.Property(o => o.UserId).IsRequired();
                entity.Property(o => o.OrderDate)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");
                entity.Property(o => o.TotalAmount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.HasOne(o => o.User)
                      .WithMany()
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(o => o.Items)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(oi => oi.Id);

                entity.Property(oi => oi.OrderId).IsRequired();
                entity.Property(oi => oi.BookId).IsRequired();
                entity.Property(oi => oi.Quantity).IsRequired();
                entity.Property(oi => oi.Price)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.HasOne(oi => oi.Book)
                      .WithMany()
                      .HasForeignKey(oi => oi.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(w => new { w.UserId, w.BookId });

                entity.HasOne(w => w.User)
                      .WithMany(u => u.WishlistItems)
                      .HasForeignKey(w => w.UserId);

                entity.HasOne(w => w.Book)
                      .WithMany()
                      .HasForeignKey(w => w.BookId);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId);

                entity.HasOne(r => r.Book)
                      .WithMany(b => b.Reviews)
                      .HasForeignKey(r => r.BookId);
            });
        }
    }
}