namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();

            var albumsInfo = context.Albums
                .Where(a => a.ProducerId.HasValue && a.ProducerId == producerId)
                .ToArray()
                .OrderByDescending(a => a.Price)
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                              .Select(s => new
                              {
                                  s.Name,
                                  Price = s.Price.ToString("F2"),
                                  WriterName = s.Writer.Name
                              })
                              .OrderByDescending(s => s.Name)
                              .ThenBy(s => s.WriterName)
                              .ToArray(),
                    TotalPrice = a.Price.ToString("F2")
                })
                .ToArray();       

            foreach (var album in albumsInfo)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");

                sb.AppendLine("-Songs:");

                int indexer = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{indexer}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price}");
                    sb.AppendLine($"---Writer: {song.WriterName}");

                    indexer++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalPrice}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var sb = new StringBuilder();

            var songsInfo = context.Songs
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers.Select(sp => new
                         {
                             FullName = $"{sp.Performer.FirstName} {sp.Performer.LastName}",
                         })
                         .OrderBy(p => p.FullName)
                         .ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToArray();

            int indexer = 1;

            foreach (var song in songsInfo)
            {
                sb.AppendLine($"-Song #{indexer}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer.FullName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration}");

                indexer++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
