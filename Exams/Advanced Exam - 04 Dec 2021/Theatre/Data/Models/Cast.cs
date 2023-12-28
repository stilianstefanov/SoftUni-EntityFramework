﻿namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.CastFullNameMaxLength)]
        public string FullName { get; set; } = null!;

        public bool IsMainCharacter { get; set; }

        public string PhoneNumber { get; set; } = null!;

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        public Play Play { get; set; } = null!;
    }
}
