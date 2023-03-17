namespace ProductShop
{
    using AutoMapper;

    using DTOs.Import;
    using DTOs.Export;
    using Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Import
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<ImportCategoryDto, Category>();

            this.CreateMap<ImportCategoryProductDto,  CategoryProduct>();

            //Export
            //Problem 5
            this.CreateMap<Product, ExportProductDto>()
                .ForMember(d => d.BuyerFullName, opt => 
                opt.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"));

            //Problem 6
            this.CreateMap<Product, ExportSoldProductDto>();

            this.CreateMap<User, ExportUserWithSoldItemDto>()
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(s => s.ProductsSold.ToArray()));

            //Problem 7
            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(d => d.Count, opt => opt.MapFrom(s => s.CategoryProducts.Count()))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(s => s.CategoryProducts.Select(cp => cp.Product.Price).Average()))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(s => s.CategoryProducts.Select(cp => cp.Product.Price).Sum()));
        }
    }
}
