namespace CarDealer
{
    using AutoMapper;
    using DTOs.Import;
    using Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Import
            this.CreateMap<ImportSupplierDto, Supplier>();
        }
    }
}
