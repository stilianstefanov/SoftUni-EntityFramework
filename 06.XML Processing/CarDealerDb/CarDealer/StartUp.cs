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

            string path = @"../../../Datasets/parts.xml";

            string input = File.ReadAllText(path);

            Console.WriteLine(ImportParts(context, input));
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

        private static IMapper InitializeMapper()
            => new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
    }
}