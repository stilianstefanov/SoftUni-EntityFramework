﻿namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [XmlElement("Name")]
        [MaxLength(ValidationConstants.FootBallerNameMaxLength)]
        [MinLength(ValidationConstants.FootBallerNameMinLength)]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        public string? ContractStartDate { get; set; }

        [XmlElement("ContractEndDate")]
        public string? ContractEndDate { get; set; }

        [XmlElement("PositionType")]
        public int PositionType { get; set; }

        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }
    }
}
