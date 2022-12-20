using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels
{
    public class CompanyFiling
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

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("website")]
        public string? Website { get; set; }

        [JsonProperty("investorWebsite")]
        public string? InvestorWebsite { get; set; }

        [JsonProperty("category")]
        public string? Category { get; set; }

        [JsonProperty("fiscalYearEnd")]
        public string? FiscalYearEnd { get; set; }

        [JsonProperty("stateOfIncorporation")]
        public string? StateOfIncorporation { get; set; }

        [JsonProperty("stateOfIncorporationDescription")]
        public string? StateOfIncorporationDescription { get; set; }

        [JsonProperty("addresses")]
        public Dictionary<string, Dictionary<string, string>>? Addresses { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("flags")]
        public string? Flags { get; set; }

        [JsonProperty("formerNames")]
        public string[]? FormerNames { get; set; }

        [JsonProperty("filings")]
        public Filings? Filings { get; set; }

    }
}

