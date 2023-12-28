namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatersWithTickets = context.Theatres
                .Where(th => th.NumberOfHalls >= numbersOfHalls && th.Tickets.Count >= 20)
                .OrderByDescending(th => th.NumberOfHalls)
                .ThenBy(th => th.Name)
                .ToArray()
                .Select(th => new
                {
                    Name = th.Name,
                    Halls = th.NumberOfHalls,
                    TotalIncome = th.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Sum(t => t.Price),
                    Tickets = th.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                                        .Select(t => new
                                        {
                                            Price = t.Price,
                                            RowNumber = t.RowNumber
                                        })
                                        .OrderByDescending(t => t.Price)
                                        .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(theatersWithTickets, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            var playDtos = context.Plays
                .ToArray()
                .Where(p => (double)p.Rating <= raiting)
                .Select(p => new ExportPlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts.Where(c => c.IsMainCharacter)
                                    .Select(c => new ExportCastDto()
                                    {
                                        FullName = c.FullName,
                                        MainCharacter = $"Plays main character in '{p.Title}'."
                                    })
                                    .OrderByDescending(c => c.FullName)
                                    .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            return Serialize<ExportPlayDto[]>(playDtos, "Plays");
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
