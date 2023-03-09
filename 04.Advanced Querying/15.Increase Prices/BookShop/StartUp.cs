namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            IncreasePrices(db);
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksBefore2010 = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in booksBefore2010)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }
    }
}


