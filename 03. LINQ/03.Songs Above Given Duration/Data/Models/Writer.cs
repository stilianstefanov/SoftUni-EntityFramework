﻿namespace MusicHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Writer
    {
        public Writer()
        {
            Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.WriterNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.WriterPseudonymMaxLength)]
        public string? Pseudonym { get; set; }

        public virtual ICollection<Song> Songs { get; set; } = null!;
    }
}
