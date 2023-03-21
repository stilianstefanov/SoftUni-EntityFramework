namespace CarDealer
{
    using AutoMapper;

    using DTOs.Import;
    using Data;
    using Utilities;
    using Models;
    using System.IO;
    using CarDealer.DTOs.Export;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //string path = @"../../../Datasets/sales.xml";

            //string input = File.ReadAllText(path);

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
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

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            XmlHelper xmlHelper= new XmlHelper();
            IMapper mapper = InitializeMapper();

            ExportCarWithDistanceDto[] carDtos = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarWithDistanceDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportCarWithDistanceDto[]>(carDtos, "cars");
        }

        //Problem 15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            XmlHelper xmlHelper= new XmlHelper();
            IMapper mapper = InitializeMapper();

            ExportCarBMWDto[] carDtos = context.Cars
                .Where(c => c.Make.ToUpper() == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportCarBMWDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportCarBMWDto[]>(carDtos, "cars");
        }

        //Problem 16 
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            IMapper mapper = InitializeMapper();

            ExportLocalSupplierDto[] supplierDtos = context.Suppliers
                .Where(s => !s.IsImporter)
                .ProjectTo<ExportLocalSupplierDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize<ExportLocalSupplierDto[]>(supplierDtos, "suppliers");
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportCarAndPartsDto[] carDtos = context.Cars
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(C => C.Model)
                .Take(5)
                .Select(c => new ExportCarAndPartsDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars.Select(pc => new ExportCarPartDto()
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .ToArray();

            return xmlHelper.Serialize<ExportCarAndPartsDto[]>(carDtos, "cars");
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var tempDto = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SalesInfo = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver
                            ? s.Car.PartsCars.Sum(p => Math.Round((double)p.Part.Price * 0.95, 2))
                            : s.Car.PartsCars.Sum(p => (double)p.Part.Price)
                    }).ToArray(),
                })
                .ToArray();

            ExportCustomerSalesDto[] totalSalesDtos = tempDto
                .OrderByDescending(t => t.SalesInfo.Sum(s => s.Prices))
                .Select(t => new ExportCustomerSalesDto()
                {
                    Name = t.FullName,
                    BoughtCars = t.BoughtCars,
                    SpentMoney = t.SalesInfo.Sum(s => s.Prices).ToString("f2")
                })
                .ToArray();

            return xmlHelper.Serialize<ExportCustomerSalesDto[]>(totalSalesDtos, "customers");
        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            ExportSaleDto[] saleDtos = context.Sales
                .Select(s => new ExportSaleDto()
                {
                    Car = new ExportSaleCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Select(pc => pc.Part.Price).Sum(),
                    PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - (s.Discount / 100))), 4)
                })
                .ToArray();

            return xmlHelper.Serialize<ExportSaleDto[]>(saleDtos, "sales");
        }

        private static IMapper InitializeMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}