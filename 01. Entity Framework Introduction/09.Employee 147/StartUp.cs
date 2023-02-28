namespace SoftUni
{
    using Data;
    using SoftUni.Models;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContxt = new SoftUniContext();

            Console.WriteLine(GetEmployee147(dbContxt));
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                                 .Select(ep => new
                                 {
                                     ProjectName = ep.Project.Name
                                 })
                                 .OrderBy(p => p.ProjectName)
                                 .ToArray()
                })
                .FirstOrDefault();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            sb.AppendLine(string.Join(Environment.NewLine, employee.Projects.Select(p => p.ProjectName)));
          
            return sb.ToString().TrimEnd();
        }
    }
}