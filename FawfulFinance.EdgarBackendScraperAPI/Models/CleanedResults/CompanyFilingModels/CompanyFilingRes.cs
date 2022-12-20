using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Models.CleanedResults.CompanyFilingModels
{
    public class CompanyFilingRes
    {
        [JsonProperty("cik")]
        public string? Cik { get; set; }

        [JsonProperty("entityType")]
        public string? EntityType { get; set; }

        [JsonProperty("sic")]
        public string? Sic { get; set; }

        [JsonProperty("sicDescription")]
        public string? SicDescription { get; set; }

        [JsonProperty("insiderTransactionForOwnerExists")]
        public int? InsiderTransactionForOwnerExists { get; set; }

        [JsonProperty("insiderTransactionForIssuerExists")]
        public int? InsiderTransactionForIssuerExists { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("tickers")]
        public string[]? Tickers { get; set; }

        [JsonProperty("exchanges")]
        public string[]? Exchanges { get; set; }

        [JsonProperty("ein")]
        public string? Ein { get; set; }

        [JsonProperty("category")]
        public string? Category { get; set; }

        [JsonProperty("fiscalYearEnd")]
        public string? FiscalYearEnd { get; set; }

        [JsonProperty("stateOfIncorporation")]
        public string? StateOfIncorporation { get; set; }

        [JsonProperty("filings")]
        public List<FilingDetail>? Filings { get; set; }

    }
}

