namespace CarDealer.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("sale")]
    public class ExportSaleDto
    {
        [XmlElement("car")]
        public ExportSaleCarDto Car { get; set; } = null!;

        [XmlElement("discount")]
        public int Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; } 

        [XmlElement("price-with-discount")]
        public double PriceWithDiscount { get; set; } 
    }
}
