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

            string path = @"..\..\..\Datasets\suppliers.json";

            string inputJson = File.ReadAllText(path);

            Console.WriteLine(ImportSuppliers(context, inputJson));
        }

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


        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}