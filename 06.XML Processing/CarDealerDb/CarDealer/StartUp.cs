namespace CarDealer
{
    using AutoMapper;

    using DTOs.Import;
    using Data;
    using Utilities;
    using Models;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //string path = @"../../../Datasets/sales.xml";

            //string input = File.ReadAllText(path);

            Console.WriteLine();
        }

        //Problem 9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportSupplierDto[] importSupplierDtos = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            ICollection<Supplier> validSuppliers = new HashSet<Supplier>();
            foreach (var dto in importSupplierDtos)
            {
                if (string.IsNullOrEmpty(dto.Name))
                {
                    continue;
                }

                validSuppliers.Add(mapper.Map<Supplier>(dto));
            }

            context.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportPartDto[] importPartDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

            ICollection<Part> validParts = new HashSet<Part>();
            foreach (var dto in importPartDtos)
            {
                if (string.IsNullOrEmpty(dto.Name))
                {
                    continue;
                }
                if (!dto.SupplierId.HasValue || !context.Suppliers.Any(s => s.Id == dto.SupplierId))
                {
                    continue;
                }

                validParts.Add(mapper.Map<Part>(dto));
            }

            context.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ImportCarDto[] importCarDtos = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

            ICollection<Car> validCars = new HashSet<Car>();
            foreach (var dto in importCarDtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                };

                foreach (var partId in dto.PartIds.DistinctBy(p => p.Id))
                {
                    if (!context.Parts.Any(p => p.Id == partId.Id))
                    {
                        continue;
                    }

                    car.PartsCars.Add(new PartCar()
                    {
                        PartId = partId.Id
                    });
                }

                validCars.Add(car);
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportCustomerDto[] customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            ICollection<Customer> customers = new HashSet<Customer>();
            foreach (var dto in customerDtos)
            {
                customers.Add(mapper.Map<Customer>(dto));
            }

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlHelper xmlHelper= new XmlHelper();
            IMapper mapper = InitializeMapper();

            ImportSaleDto[] importSaleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            ICollection<Sale> sales = new HashSet<Sale>();
            foreach (var dto in importSaleDtos)
            {
                if (!context.Cars.Any(c => c.Id == dto.CarId))
                {
                    continue;
                }

                sales.Add(mapper.Map<Sale>(dto));
            }

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        private static IMapper InitializeMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}