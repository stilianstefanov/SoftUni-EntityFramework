namespace CarDealer.DTOs.Import
{
    using System.Xml.Serialization;

    [XmlType("partId")]
    public class ImportCarPartIdDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
