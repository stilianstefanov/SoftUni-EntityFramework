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

            Console.WriteLine(RemoveBooks(db));
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var countOfDeletedBooks = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(countOfDeletedBooks);

            context.SaveChanges();

            return countOfDeletedBooks.Count;
        }
    }
}


