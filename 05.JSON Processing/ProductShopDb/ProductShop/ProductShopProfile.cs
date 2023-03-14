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
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<ImportCategoryDto, Category>();

            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();

            this.CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(d => d.SellerFullName, opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            this.CreateMap<Product, ExportSoldProductDto>()
                .ForMember(d => d.BuyerFirstName, opt => opt.MapFrom(s => s.Buyer!.FirstName))
                .ForMember(d => d.BuyerLastName, opt => opt.MapFrom(s => s.Buyer!.LastName));

            this.CreateMap<User, ExportUserWithSoldItemDto>()
                .ForMember(d => d.ProductsSold, opt => opt.MapFrom(s => s.ProductsSold.Where(ps => ps.Buyer != null)));

            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(d => d.ProductsCount, opt => opt.MapFrom(s => s.CategoriesProducts.Count))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(s => s.CategoriesProducts.Average(p => p.Product.Price).ToString("F2")))
                .ForMember(d => d.TotalRavenue, opt => opt.MapFrom(s => s.CategoriesProducts.Sum(p => p.Product.Price).ToString("F2")));
        }       
    }
}
