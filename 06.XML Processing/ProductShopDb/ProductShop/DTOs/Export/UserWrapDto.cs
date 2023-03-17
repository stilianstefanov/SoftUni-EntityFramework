namespace ProductShop.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("Users")]
    public class UserWrapDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("users")]
        public UserDto[] Users { get; set; } = null!;
    }
}
