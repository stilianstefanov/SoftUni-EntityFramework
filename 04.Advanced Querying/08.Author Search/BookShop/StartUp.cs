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
            
            Console.WriteLine(GetAuthorNamesEndingIn(db, input));
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {            
            var authorsNames = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .AsEnumerable()
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a.FullName)                
                .ToArray();

            return string.Join(Environment.NewLine, authorsNames.Select(a => a.FullName));
        }
    }
}


