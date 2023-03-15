namespace CarDealer
{
    using Newtonsoft.Json;
    using AutoMapper;

    using DTOs.Import;
    using Data;   
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            string path = @"..\..\..\Datasets\customers.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(ImportCustomers(context, inputJson));
        }

        //Problem 9
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSupplierDto[] supplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson)!;

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (var sDto in supplierDtos)
            {
                suppliers.Add(mapper.Map<Supplier>(sDto));
            }

            context.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportPartDto[] importPartDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson)!;

            ICollection<Part> parts = new HashSet<Part>();

            foreach (var ipDto in importPartDtos)
            {
                if (!context.Suppliers.Any(s => s.Id == ipDto.SupplierId))
                {
                    continue;
                }

                parts.Add(mapper.Map<Part>(ipDto));
            }

            context.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {            
            ImportCarDto[] importCarDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson)!;

            ICollection<Car> cars = new HashSet<Car>();
            ICollection<PartCar> parts = new HashSet<PartCar>();

            foreach (var icDto in importCarDtos)
            {
                Car newCar = new Car()
                {
                    Make = icDto.Make,
                    Model = icDto.Model,
                    TravelledDistance = icDto.TravelledDistance,
                };

                cars.Add(newCar);

                foreach (var partId in icDto.PartsCarsIds.Distinct())
                {
                    parts.Add(new PartCar()
                    {
                        Car = newCar,
                        PartId = partId
                    });
                }                              
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportCustomerDto[] importCustomerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson)!;

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (var icDto in importCustomerDtos)
            {
                customers.Add(mapper.Map<Customer>(icDto));
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}