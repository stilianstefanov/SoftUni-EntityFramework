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

            Console.WriteLine(GetMostRecentBooks(db));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categoriesInfo = context.Categories
                .Select(c => new
                {
                    c.Name,
                    RecentBooks = c.CategoryBooks
                                      .OrderByDescending(cb => cb.Book.ReleaseDate)
                                      .Take(3)
                                      .Select(cb => new
                                      {
                                          BookName = cb.Book.Title,
                                          ReleaseDate = cb.Book.ReleaseDate!.Value.Year
                                      })
                                      .ToArray()
                })
                .OrderBy(c => c.Name)
                .ToArray();

            foreach (var category in categoriesInfo)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.RecentBooks)
                {
                    sb.AppendLine($"{book.BookName} ({book.ReleaseDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}


