namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;
    using ProductShop.Models;

    public class ExportUserWithSoldItemDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("soldProducts")]
        public ICollection<ExportSoldProductDto> ProductsSold { get; set; } = null!;
    }
}
