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


            Console.WriteLine(DeleteProjectById(dbContxt));
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var employeesProjectsToDelete = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            context.EmployeesProjects
                .RemoveRange(employeesProjectsToDelete);

            var projectToDelete = context.Projects
                .Where(p => p.ProjectId == 2);

            context.Projects
                .RemoveRange(projectToDelete);

            context.SaveChanges();

            string[] projectsNames = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            return string.Join(Environment.NewLine, projectsNames);
        }
    }
}