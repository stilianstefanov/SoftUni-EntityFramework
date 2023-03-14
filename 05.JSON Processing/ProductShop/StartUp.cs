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

            string path = @"..\..\..\Datasets\users.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(ImportUsers(context, inputJson));
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