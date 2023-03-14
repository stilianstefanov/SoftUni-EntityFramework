namespace ProductShop
{
    using AutoMapper;
    using Newtonsoft.Json;

    using Data;
    using DTOs.Import;
    using ProductShop.Models;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            string path = @"..\..\..\Datasets\products.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(ImportProducts(context, inputJson));
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