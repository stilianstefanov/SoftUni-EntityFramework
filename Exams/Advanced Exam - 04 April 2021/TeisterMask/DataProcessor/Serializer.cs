﻿namespace TeisterMask.DataProcessor
{
    using Data;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projectDtos = context.Projects
                .Where(p => p.Tasks.Any())
                .ToArray()
                .Select(p => new ExportProjectDto
                {
                    ProjectName = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.Select(t => new ExportTaskDto()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            return Serialize<ExportProjectDto[]>(projectDtos, "Projects");
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Where(et => et.Task.OpenDate >= date)
                                            .OrderByDescending(et => et.Task.DueDate)
                                            .ThenBy(et => et.Task.Name)
                                            .Select(et => new
                                            {
                                                TaskName = et.Task.Name,
                                                OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                                                DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                                                LabelType = et.Task.LabelType.ToString(),
                                                ExecutionType = et.Task.ExecutionType.ToString()
                                            })
                                            .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Count())
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }

        private static string Serialize<T>(T obj, string rootName)
        {
            var sb = new StringBuilder();

            var xmlRoot =
                new XmlRootAttribute(rootName);
            var xmlSerializer =
                new XmlSerializer(typeof(T), xmlRoot);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using var writer = new StringWriter(sb);
            xmlSerializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}