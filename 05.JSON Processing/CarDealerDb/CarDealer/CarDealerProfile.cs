namespace CarDealer
{
    using AutoMapper;

    using Models;
    using DTOs.Import;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSupplierDto, Supplier>();
        }
    }
}
