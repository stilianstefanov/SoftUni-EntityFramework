namespace CarDealer
{
    using AutoMapper;

    using DTOs.Import;
    using Data;
    using Utilities;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            string path = @"../../../Datasets/suppliers.xml";

            string input = File.ReadAllText(path);

            Console.WriteLine(ImportSuppliers(context, input));
        }

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



        private static IMapper InitializeMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}