namespace P02_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class User
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.UserPasswordMaxLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.UserEmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.UserNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Balance { get; set; }

        public ICollection<Bet> Bets { get; set; } = null!;
    }
}
