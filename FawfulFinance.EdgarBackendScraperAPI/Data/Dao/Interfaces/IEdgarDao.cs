using FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels;

namespace FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces
{
    public interface IEdgarDao
    {
        public Task<CompanyFiling> GetCompanyFilings(string cikNumber);
        public Task<string> GetReportRawText(string cikNumber, string filingNumber);
        public Task<Dictionary<string, string>> GetMasterReportUrls(string cikNumber, string filingNumber);
        public Task<string> GetFinancialStatement(string url);
    }
}

