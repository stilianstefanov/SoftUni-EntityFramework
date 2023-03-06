namespace P02_FootballBetting.Data.Models
{   
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common;

    public class Town
    {
        public Town()
        {
            Teams = new HashSet<Team>();
        }

        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TownNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public Country Country { get; set; } = null!;

        public ICollection<Team> Teams { get; set; } = null!;
    }
}
