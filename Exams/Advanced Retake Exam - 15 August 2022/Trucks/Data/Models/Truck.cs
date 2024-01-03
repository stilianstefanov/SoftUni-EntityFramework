﻿namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Trucks.Data.Models.Enums;

    public class Truck
    {
        public Truck()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.TruckRegistrationNumberLength)]
        public string RegistrationNumber { get; set; } = null!;

        [MaxLength(ValidationConstants.TruckVinNumberLength)]
        public string VinNumber { get; set; } = null!;

        public int TankCapacity { get; set; }
        
        public int CargoCapacity { get; set; }
   
        public CategoryType CategoryType { get; set; }
    
        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }
        public Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = null!;
    }
}