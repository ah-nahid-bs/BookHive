using BookHive.Models;

namespace BookHive.Data;
public static class DbInitializer
{
    public static void Seed(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        context.Database.EnsureCreated();

        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Fiction" },
                new Category { Name = "Science" },
                new Category { Name = "Biography" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        if (!context.Books.Any())
        {
            var books = new List<Book>
            {
                new Book { Title = "The Alchemist", Author = "Paulo Coelho", Price = 15.99M, ImageUrl = "/images/book1.jpg", CategoryId = 1 },
                new Book { Title = "Brief History of Time", Author = "Stephen Hawking", Price = 20.00M, ImageUrl = "/images/book2.jpg", CategoryId = 2 },
                new Book { Title = "Steve Jobs", Author = "Walter Isaacson", Price = 18.50M, ImageUrl = "/images/book3.jpg", CategoryId = 3 }
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
