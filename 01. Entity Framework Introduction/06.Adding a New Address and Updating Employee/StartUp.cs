namespace SoftUni
{
    using Data;
    using SoftUni.Models;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContxt = new SoftUniContext();

            Console.WriteLine(AddNewAddressToEmployee(dbContxt));
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAdress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employeeToUpdate = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");

            employeeToUpdate.Address = newAdress;

            context.SaveChanges();

            var sb = new StringBuilder();

            var employees = context.Employees
                .Select(x => new
                {
                    x.AddressId,
                    x.Address.AddressText
                })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine(employee.AddressText);
            }

            return sb.ToString().TrimEnd();
        }
    }
}