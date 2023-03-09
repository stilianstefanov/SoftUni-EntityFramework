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

            string input = Console.ReadLine()!;
            
            Console.WriteLine(CountBooks(db, int.Parse(input)));
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int countOfBooks = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return countOfBooks;
        }
    }
}


