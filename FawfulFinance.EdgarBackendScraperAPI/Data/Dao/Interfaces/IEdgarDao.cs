using FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels;

namespace FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces
{
    public interface IEdgarDao
    {
        public Task<CompanyFiling> GetCompanyFilings(string cikNumber);
    }
}

