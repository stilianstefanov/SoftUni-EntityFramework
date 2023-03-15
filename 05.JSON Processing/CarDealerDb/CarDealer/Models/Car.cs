namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            Sales = new HashSet<Sale>();
            PartsCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TravelledDistance { get; set; }

        public ICollection<Sale> Sales { get; set; } = null!;

        public ICollection<PartCar> PartsCars { get; set; } = null!;
    }
}
