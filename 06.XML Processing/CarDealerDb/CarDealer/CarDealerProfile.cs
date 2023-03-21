namespace CarDealer
{
    using AutoMapper;
    using DTOs.Export;
    using DTOs.Import;
    using Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Import
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId, opt => opt.MapFrom(s => s.SupplierId!.Value));

            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate)));

            this.CreateMap<ImportSaleDto, Sale>();

            //Export

            //Problem 14
            this.CreateMap<Car, ExportCarWithDistanceDto>();
            
            //Problem 15
            this.CreateMap<Car, ExportCarBMWDto>();

            //problem 16
            this.CreateMap<Supplier, ExportLocalSupplierDto>()
                .ForMember(d => d.PartsCount, opt => opt.MapFrom(d => d.Parts.Count));
        }
    }
}
