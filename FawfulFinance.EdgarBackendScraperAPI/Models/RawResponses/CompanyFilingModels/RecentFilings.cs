using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels
{
    public class RecentFilings
    {
        [JsonProperty("accessionNumber")]
        public string[]? AccessionNumber { get; set; }

        [JsonProperty("filingDate")]
        public string[]? FilingDate { get; set; }

        [JsonProperty("reportDate")]
        public string[]? ReportDate { get; set; }

        [JsonProperty("acceptanceDateTime")]
        public string[]? AcceptanceDateTime { get; set; }

        [JsonProperty("act")]
        public string[]? Act { get; set; }

        [JsonProperty("form")]
        public string[]? Form { get; set; }

        [JsonProperty("fileNumber")]
        public string[]? FileNumber { get; set; }

        [JsonProperty("filmNumber")]
        public string[]? FilmNumber { get; set; }

        [JsonProperty("items")]
        public string[]? Items { get; set; }

        [JsonProperty("size")]
        public string[]? Size { get; set; }

        [JsonProperty("isXBRL")]
        public string[]? IsXbrl { get; set; }

        [JsonProperty("isInlineXBRL")]
        public string[]? IsInlineXBRL { get; set; }

        [JsonProperty("primaryDocument")]
        public string[]? PrimaryDocument { get; set; }

        [JsonProperty("primaryDocDescription")]
        public string[]? PrimaryDocDescription { get; set; }
    }
}

