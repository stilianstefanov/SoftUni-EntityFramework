namespace P02_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common;
    
    public class Color
    {
        public Color()
        {
            PrimaryKitTeams = new HashSet<Team>();
            SecondaryKitTeams = new HashSet<Team>();
        }

        [Key]
        public int ColorId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.ColorNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Team> PrimaryKitTeams { get; set; } = null!;

        public ICollection<Team> SecondaryKitTeams { get; set; } = null!;
    }
}
