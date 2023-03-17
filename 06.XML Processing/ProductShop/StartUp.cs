namespace ProductShop
{
    using AutoMapper;

    using Data;
    using DTOs.Import;
    using Models;
    using Utilities;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            string path = @"..\..\..\Datasets\categories-products.xml";

            string input = File.ReadAllText(path);

            Console.WriteLine(ImportCategoryProducts(context, input));
        }

        //Problem 1
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportUserDto[] importUserDtos = xmlHelper.Deserialize<ImportUserDto[]>(inputXml, "Users");

            ICollection<User> validUsers = new HashSet<User>();
            foreach (var dto in importUserDtos)
            {
                validUsers.Add(mapper.Map<User>(dto));
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }

        //Problem 2
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportProductDto[] importProductDtos = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            ICollection<Product> validProducts = new HashSet<Product>();
            foreach (var dto in importProductDtos)
            {
                validProducts.Add(mapper.Map<Product>(dto));
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        //Problem 3
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportCategoryDto[] importCategoryDtos = xmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            ICollection<Category> validCateogries = new HashSet<Category>();
            foreach (var dto in importCategoryDtos)
            {
                if (string.IsNullOrEmpty(dto.Name))
                {
                    continue;
                }

                validCateogries.Add(mapper.Map<Category>(dto));
            }

            context.Categories.AddRange(validCateogries);
            context.SaveChanges();

            return $"Successfully imported {validCateogries.Count}";
        }

        //Problem 4
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportCategoryProductDto[] importCategoryProductDtos =
                xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();
            foreach (var dto in importCategoryProductDtos)
            {
                if (!context.Categories.Any(c => c.Id == dto.CategoryId)
                    || !context.Products.Any(p => p.Id == dto.ProductId))
                {
                    continue;
                }

                validCategoryProducts.Add(mapper.Map<CategoryProduct>(dto));
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }

        private static IMapper InitializeMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
    }
}