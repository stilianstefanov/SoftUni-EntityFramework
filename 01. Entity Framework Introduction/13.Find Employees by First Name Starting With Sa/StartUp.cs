namespace SoftUni
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using SoftUni.Models;
    using System.Globalization;
    using System.Text;
    using System.Data;


    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContxt = new SoftUniContext();


            Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(dbContxt));
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}