using System;
using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels;
using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Data.Dao
{
    public class EdgarDao : IEdgarDao
    {
        private readonly string _submissionsUri;
        private readonly string _filingUri;
        private readonly HttpClient _client;


        public EdgarDao()
        {
            _submissionsUri = @"https://data.sec.gov/submissions/CIK{0}.json";
            _filingUri = @"https://www.sec.gov/Archives/edgar/data/{0}/{1}.txt";
            _client = new HttpClient();

            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                $"FawfulFinance thewisekingsolomon82@gmail.com");
        }

        public async Task<CompanyFiling> GetCompanyFilings(string cikNumber)
        {
            string url = string.Format(_submissionsUri, cikNumber);
            using HttpResponseMessage response = await _client.GetAsync(url);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (jsonResponse == null)
                throw new Exception("Response is null for requested CIK for company filings");

            return JsonConvert.DeserializeObject<CompanyFiling>(jsonResponse);
        }

        public async Task<string> GetReportRawText(string cikNumber, string filingNumber)
        {
            string url = string.Format(_filingUri, cikNumber, filingNumber);
            using HttpResponseMessage response = await _client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}

