namespace CarDealer
{
    using Newtonsoft.Json;
    using AutoMapper;

    using DTOs.Import;
    using Data;   
    using Models;
    using Newtonsoft.Json.Serialization;
    using System.Globalization;
    using System.Xml.Linq;
    using System.Diagnostics;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            string path = @"..\..\..\Datasets\sales.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
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

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSaleDto[] importSaleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson)!;

            ICollection<Sale> sales = new HashSet<Sale>();

            foreach (var isDto in importSaleDtos)
            {
                sales.Add(mapper.Map<Sale>(isDto));
            }

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //Problem 14
        public static string GetOrderedCustomers(CarDealerContext context)
        {            
            var orderedCustomers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    c.IsYoungDriver
                })
                .ToArray();

            return JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);
        }

        //Problem 15 - Judge has wrong test for this method
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsFromMakeToyota = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance
                })
                .ToArray();

            return JsonConvert.SerializeObject(carsFromMakeToyota, Formatting.Indented);
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            return JsonConvert.SerializeObject (suppliers, Formatting.Indented);
        }

        //Problem 17 - Judge has wrong test for this method
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsAndParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartsCars
                        .Select(p => new
                        {
                            p.Part.Name,
                            Price = $"{p.Part.Price:f2}"
                        })
                })
                .ToArray();

            return JsonConvert.SerializeObject(carsAndParts, Formatting.Indented);
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var customerSales = context.Customers
                 .Where(c => c.Sales.Any())
                 .Select(c => new
                 {
                     FullName = c.Name,
                     BoughtCars = c.Sales.Count(),
                     SalePrices = c.Sales.SelectMany(x => x.Car.PartsCars.Select(x => x.Part.Price))
                 })
                 .ToArray();

            var totalSalesByCustomer = customerSales.Select(t => new
            {
                t.FullName,
                t.BoughtCars,
                SpentMoney = t.SalePrices.Sum()
            })
            .OrderByDescending(t => t.SpentMoney)
            .ThenByDescending(t => t.BoughtCars)
            .ToArray();
        
            return JsonConvert.SerializeObject(totalSalesByCustomer, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver
            });
        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesWithDiscount = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TravelledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = $"{s.Discount:f2}",
                    price = $"{s.Car.PartsCars.Sum(p => p.Part.Price):f2}",
                    priceWithDiscount = $"{s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - s.Discount / 100):f2}"
                })
                .ToArray();

            return JsonConvert.SerializeObject(salesWithDiscount, Formatting.Indented);
        }

        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }

        private static IContractResolver ConfigureCamelCaseNaming()
        {
            IContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };

            return contractResolver;
        }
    }
}