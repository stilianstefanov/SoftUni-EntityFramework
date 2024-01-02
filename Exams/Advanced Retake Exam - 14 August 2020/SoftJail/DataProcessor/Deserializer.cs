namespace SoftJail.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        private const string SuccessfullyImportedDepartment = "Imported {0} with {1} cells";

        private const string SuccessfullyImportedPrisoner = "Imported {0} {1} years old";

        private const string SuccessfullyImportedOfficer = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString)!;

            ICollection<Department> validDepartments = new HashSet<Department>();
            foreach (var departmentDto in departmentDtos)
            {
                if (!IsValid(departmentDto) || departmentDto.Cells == null || !departmentDto.Cells.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (departmentDto.Cells.Any(c => !IsValid(c)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var department = new Department()
                {
                    Name = departmentDto.Name
                };

                foreach (var cellDto in departmentDto.Cells)
                {
                    department.Cells.Add(new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    });
                }

                validDepartments.Add(department);
                sb.AppendLine(string.Format(SuccessfullyImportedDepartment, department.Name, department.Cells.Count));
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString)!;

            ICollection<Prisoner> validPrisoners = new HashSet<Prisoner>();
            foreach (var prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto) || prisonerDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isIncarcerationDateValid = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var incarcerationDate);

                if (!isIncarcerationDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? releaseDate = null;
                if (!string.IsNullOrEmpty(prisonerDto.ReleaseDate))
                {
                    var isReleaseDateValid = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var releaseDateResult);

                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    releaseDate = releaseDateResult;
                }

                if (prisonerDto.Bail.HasValue)
                {
                    if (prisonerDto.Bail < 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                }

                var prisoner = new Prisoner()
                {
                    FullName = prisonerDto.FullName!,
                    Nickname = prisonerDto.Nickname!,
                    Age = prisonerDto.Age!.Value,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };

                foreach (var mailDto in prisonerDto.Mails)
                {
                    prisoner.Mails.Add(new Mail()
                    {
                        Description = mailDto.Description!,
                        Sender = mailDto.Sender!,
                        Address = mailDto.Address!
                    });
                }

                validPrisoners.Add(prisoner);
                sb.AppendLine(string.Format(SuccessfullyImportedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var officerDtos = Deserialize<ImportOfficerDto[]>(xmlString, "Officers");

            ICollection<Officer> validOfficers = new HashSet<Officer>();
            foreach (var officerDto in officerDtos)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (officerDto.Salary < 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isPositionValid = Enum.TryParse<Position>(officerDto.Position, out var position);
                var isWeaponValid = Enum.TryParse<Weapon>(officerDto.Weapon, out var weapon);

                if (!isPositionValid || !isWeaponValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = new Officer()
                {
                    FullName = officerDto.FullName!,
                    Salary = officerDto.Salary!.Value,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officerDto.DepartmentId!.Value
                };

                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    officer.OfficerPrisoners.Add(new OfficerPrisoner()
                    {
                        PrisonerId = int.Parse(prisonerDto.Id),
                        Officer = officer
                    });
                }

                validOfficers.Add(officer);
                sb.AppendLine(string.Format(SuccessfullyImportedOfficer, officer.FullName, officer.OfficerPrisoners.Count));
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            var xmlRoot = new XmlRootAttribute(rootName);
            var xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            using var reader = new StringReader(inputXml);
            var deserializedDtos =
                (T)xmlSerializer.Deserialize(reader);

            return deserializedDtos;
        }
    }
}