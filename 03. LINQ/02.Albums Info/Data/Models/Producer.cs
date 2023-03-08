namespace MusicHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Producer
    {
        public Producer()
        {
            Albums = new HashSet<Album>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.ProducerNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.ProducerPseudonymMaxLength)]
        public string? Pseudonym { get; set; }

        [MaxLength(ValidationConstants.ProducerPhoneNumberMaxLength)]
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Album> Albums { get; set; } = null!;
    }
}
