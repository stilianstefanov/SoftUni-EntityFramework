namespace ProductShop.DTOs.Export
{
    using Newtonsoft.Json;

    public class ExportCategoryDto
    {
        [JsonProperty("category")]
        public string Name { get; set; } = null!;

        [JsonProperty("productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; } = null!;

        [JsonProperty("totalRevenue")]
        public string TotalRavenue { get; set; } = null !;
    }
}
