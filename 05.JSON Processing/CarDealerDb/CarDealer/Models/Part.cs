namespace CarDealer.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Part
    {
        public Part()
        {
            PartsCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!; 

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(Supplier))]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public ICollection<PartCar> PartsCars { get; set; } = null!;
    }
}
