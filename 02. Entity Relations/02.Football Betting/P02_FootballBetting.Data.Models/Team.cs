namespace P02_FootballBetting.Data.Models
{    
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Common;

    public class Team
    {
        public Team()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.TeamUrlMaxLength)]
        public string? LogoUrl { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TeamInitialsMaxLength)]
        public string Initials { get; set; } = null!;

        [Required]
        public decimal Budget { get; set; }

        [Required]        
        public int PrimaryKitColorId { get; set; }

        public Color PrimaryKitColor { get; set; } = null!;

        [Required]        
        public int SecondaryKitColorId { get; set; }

        public Color SecondaryKitColor { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }

        public Town Town { get; set; } = null!;

        public ICollection<Game> HomeGames { get; set; } = null!;

        public ICollection<Game> AwayGames { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = null!;
    }
}