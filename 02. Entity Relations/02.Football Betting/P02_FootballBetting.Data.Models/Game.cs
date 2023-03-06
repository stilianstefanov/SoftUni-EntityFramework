namespace P02_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common;
    
    public class Game
    {
        public Game()
        {
            PlayersStatistics = new HashSet<PlayerStatistic>();
        }

        [Key]
        public int GameId { get; set; }

        [Required]
        public int HomeTeamId { get; set; }

        public Team HomeTeam { get; set; } = null!;

        [Required]
        public int AwayTeamId { get; set; }

        public Team AwayTeam { get; set; } = null!;

        [Required]
        public int HomeTeamGoals { get; set; }

        [Required]
        public int AwayTeamGoals { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public double HomeTeamBetRate { get; set; }

        [Required]
        public double AwayTeamBetRate { get; set; }

        [Required]
        public double DrawBetRate { get; set; }

        [MaxLength(ValidationConstants.ResultMaxLength)]
        public string? Result { get; set; }

        public ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;

        public ICollection<Bet> Bets { get; set; } = null!;
    }
}
