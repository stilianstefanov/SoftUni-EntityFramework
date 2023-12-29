namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var dtos = context.Coaches
                .Where(c => c.Footballers.Any())
                .ToArray()
                .Select(c => new ExportCoachDto()
                {
                    CoachName = c.Name,
                    FootballersCount = c.Footballers.Count,
                    Footballers = c.Footballers
                                   .Select(f => new ExportFootBallerDto()
                                   {
                                       Name = f.Name,
                                       Position = f.PositionType.ToString()
                                   })
                                   .OrderBy(f => f.Name)
                                   .ToArray()
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            return Serialize<ExportCoachDto[]>(dtos, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                                    .ThenBy(tf => tf.Footballer.Name)
                                    .Select(tf => new
                                    {
                                        FootballerName = tf.Footballer.Name,
                                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                                        PositionType = tf.Footballer.PositionType.ToString()
                                    })
                                    .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
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
