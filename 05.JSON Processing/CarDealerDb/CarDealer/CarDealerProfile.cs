namespace CarDealer
{
    using AutoMapper;

    using Models;
    using DTOs.Import;
    using CarDealer.Data;

    public class CarDealerProfile : Profile
    {
        
        public CarDealerProfile()
        {           
            this.CreateMap<ImportSupplierDto, Supplier>();

            this.CreateMap<ImportPartDto, Part>();

            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
