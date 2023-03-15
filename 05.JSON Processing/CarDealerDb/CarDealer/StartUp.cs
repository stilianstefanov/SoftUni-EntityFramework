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

            string path = @"..\..\..\Datasets\parts.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(ImportParts(context, inputJson));
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


        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}