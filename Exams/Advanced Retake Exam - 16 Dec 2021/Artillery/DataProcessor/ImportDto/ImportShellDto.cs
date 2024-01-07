﻿namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;


    [XmlType("Shell")]
    public class ImportShellDto
    {
        [Range(ValidationConstants.ShellWeightMinValue, ValidationConstants.ShellWeightMaxValue)]
        public double ShellWeight { get; set; }

        [MinLength(ValidationConstants.ShellCaliberMinLength)]
        [MaxLength(ValidationConstants.ShellCaliberMaxLength)]
        public string? Caliber { get; set; }
    }
}
