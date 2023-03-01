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


            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(dbContxt));
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departments = context.Departments
                .Where(de => de.Employees.Count > 5)
                .Select(de => new
                {
                    de.Name,
                    ManagerFirstName = de.Manager.FirstName,
                    ManagerLastName = de.Manager.LastName,
                    Employees = de.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray()
                })
                .OrderBy(de => de.Employees.Count())
                .ThenBy(de => de.Name)
                .ToArray();

            foreach (var dep in departments)
            {
                sb.AppendLine($"{dep.Name} - {dep.ManagerFirstName} {dep.ManagerLastName}");

                foreach (var emp in dep.Employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}