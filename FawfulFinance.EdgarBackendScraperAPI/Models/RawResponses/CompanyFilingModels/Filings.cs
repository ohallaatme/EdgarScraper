using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels
{
    public class Filings
    {
        [JsonProperty("recent")]
        public RecentFilings? Recent { get; set; }

        [JsonProperty("files")]
        public File[]? Files { get; set; }
    }
}

