﻿namespace Artillery.Data.Models
{
    using Artillery.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        public int GunWeight { get; set; }

        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        public int Range { get; set; }

        public GunType GunType { get; set; }

        [ForeignKey(nameof(Manufacturer))]
        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; } = null!;

        [ForeignKey(nameof(Shell))]
        public int ShellId { get; set; }

        public Shell Shell { get; set; } = null!;

        public virtual ICollection<CountryGun> CountriesGuns { get; set; } = null!;
    }
}
