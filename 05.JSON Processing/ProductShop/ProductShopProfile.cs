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
        }       
    }
}
