namespace ProductShop
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using DTOs.Import;
    using Models;
    using ProductShop.DTOs.Export;
    using Utilities;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //string path = @"..\..\..\Datasets\categories-products.xml";

            //string input = File.ReadAllText(path);

            Console.WriteLine(GetUsersWithProducts(context));
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

        //Problem 5
        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            var productsInRange = context.Products
                 .Where(p => p.Price >= 500 && p.Price <= 1000)
                 .OrderBy(p => p.Price)
                 .Take(10)
                 .ProjectTo<ExportProductDto>(mapper.ConfigurationProvider)
                 .ToArray();

            return xmlHelper.Serialize<ExportProductDto[]>(productsInRange, "Products");
        }

        //Problem 6
        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            //Manual Mapping
            //ExportUserWithSoldItemDto[] users = context.Users
            //    .Where(u => u.ProductsSold.Any())
            //    .OrderBy(u => u.LastName)
            //    .ThenBy(u => u.FirstName)
            //    .Take(5)              
            //    .Select(u => new ExportUserWithSoldItemDto()
            //    {
            //        FirstName = u.FirstName,
            //        LastName = u.LastName,
            //        SoldProducts = u.ProductsSold.Select(p => new ExportSoldProductDto()
            //        {
            //            Name = p.Name,
            //            Price = p.Price
            //        })
            //        .ToArray()                   
            //    })
            //    .ToArray();

            ExportUserWithSoldItemDto[] users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserWithSoldItemDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportUserWithSoldItemDto[]>(users, "Users");
        }

        //Problem 7
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            //Manual mapping
            //ExportCategoryDto[] categoryDtos = context.Categories
            //    .Select(c => new ExportCategoryDto()
            //    {
            //        Name = c.Name,
            //        Count = c.CategoryProducts.Count(),
            //        AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
            //        TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum(),
            //    })
            //    .OrderByDescending(c => c.Count)
            //    .ThenBy(c => c.TotalRevenue)
            //    .ToArray();

            ExportCategoryDto[] categoryDtos = context.Categories
                .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return xmlHelper.Serialize<ExportCategoryDto[]>(categoryDtos, "Categories");
        }

        //Problem 8
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var users = context
                       .Users
                       .Where(u => u.ProductsSold.Any())
                       .OrderByDescending(u => u.ProductsSold.Count)
                       .Select(u => new UserDto()
                       {
                           FirstName = u.FirstName,
                           LastName = u.LastName,
                           Age = u.Age,
                           SoldProducts = new ProductWrapDto
                           {
                               Count = u.ProductsSold.Count,
                               Products = u.ProductsSold
                                             .Select(p => new ProductDto()
                                             {
                                                 Name = p.Name,
                                                 Price = p.Price,
                                             })
                                             .OrderByDescending(p => p.Price)
                                             .ToArray()
                           }
                       })
                       .Take(10)
                       .ToArray();

            UserWrapDto userWrapDto = new UserWrapDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = users
            };

            return xmlHelper.Serialize<UserWrapDto>(userWrapDto, "Users");
        }

        private static IMapper InitializeMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
    }
}