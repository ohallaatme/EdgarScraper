using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels
{
    public class File
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("filingCount")]
        public int? FilingCount { get; set; }

        [JsonProperty("filingFrom")]
        public string? FilingFrom { get; set; }

        [JsonProperty("filingTo")]
        public string? FilingTo { get; set; }
    }
}

