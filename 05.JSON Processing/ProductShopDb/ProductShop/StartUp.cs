namespace ProductShop
{
    using AutoMapper;
    using Newtonsoft.Json;

    using Data;
    using DTOs.Import;
    using ProductShop.Models;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using ProductShop.DTOs.Export;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            string path = @"..\..\..\Datasets\categories-products.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(GetCategoriesByProductsCount(context));
        }

        //Problem 1

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson)!;

            ICollection<User> validUsers = new HashSet<User>();

            foreach (var userDto in userDtos)
            {
                validUsers.Add(mapper.Map<User>(userDto));
            }

            context.Users.AddRange(validUsers);

            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        //Problem 2

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson)!;

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach (var productDto in productDtos)
            {
                validProducts.Add(mapper.Map<Product>(productDto));
            }

            context.Products.AddRange(validProducts);

            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        //Problem 3

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson)!;

            ICollection<Category> validCategories = new HashSet<Category>();

            foreach (var categoryDto in categoryDtos)
            {
                if (string.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                validCategories.Add(mapper.Map<Category>(categoryDto));
            }

            context.Categories.AddRange(validCategories);

            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        //Problem 4

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCategoryProductDto[] categoryProductDtos = JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson)!;

            ICollection<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

            foreach (var cpd in categoryProductDtos)
            {
                categoryProducts.Add(mapper.Map<CategoryProduct>(cpd));
            }

            context.CategoriesProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        //Problem 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Include(p => p.Seller)
                .AsNoTracking()
                .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        //Problem 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var usersWithProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Include(p => p.ProductsSold)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ExportUserWithSoldItemDto> (mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToArray();


            return JsonConvert.SerializeObject (usersWithProducts, Formatting.Indented);
        }

        //Problem 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var categories = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count())
                .Include(p => p.CategoriesProducts)               
                .ProjectTo<ExportCategoryDto> (mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }
        private static IMapper CreateMapper()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();                
            }));

            return mapper;
        }
    }
}