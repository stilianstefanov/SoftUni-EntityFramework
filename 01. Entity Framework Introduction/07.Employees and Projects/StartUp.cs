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

            Console.WriteLine(GetEmployeesInPeriod(dbContxt));
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Take(10)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects
                                 .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                                 .Select(ep => new
                                 {
                                     ProjectName = ep.Project.Name,
                                     StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                                     EndDate = ep.Project.EndDate != null 
                                     ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) 
                                     : "not finished"
                                 })
                })
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                if (employee.Projects.Any())
                {
                    foreach (var project in employee.Projects)
                    {
                        sb.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}