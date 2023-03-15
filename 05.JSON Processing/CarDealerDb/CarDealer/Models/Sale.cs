using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public decimal Discount { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; 
    }
}
