namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            int year = int.Parse(Console.ReadLine()!); 

            Console.WriteLine(GetBooksNotReleasedIn(db, year));
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksInfo = context.Books
                .Where(b => !b.ReleaseDate.HasValue || b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, booksInfo.Select(b => b.Title));
        }
    }
}


