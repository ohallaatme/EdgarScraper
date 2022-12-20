using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;
using FawfulFinance.EdgarBackendScraperAPI.Models.CleanedResults.CompanyFilingModels;

namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface ICompanyFilingsService
    {
        public Task<CompanyFilingRes> GetCompanyFiling(string cikNumber);

        public Task<List<FilingDetail>> GetReportFilings(string cikNumber, string type);

        public Task<List<string>> GetAvailableFilings(string cikNumber);
    }
}

