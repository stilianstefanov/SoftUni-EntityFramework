﻿namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ImportCountryDto
    {
        [XmlElement("CountryName")]
        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        [MinLength(ValidationConstants.CountryNameMinLength)]
        public string CountryName { get; set; } = null!;

        [XmlElement("ArmySize")]
        [Range(ValidationConstants.CountryArmySizeMinValue, ValidationConstants.CountryArmySizeMaxValue)]
        public int ArmySize { get; set; }
    }
}
