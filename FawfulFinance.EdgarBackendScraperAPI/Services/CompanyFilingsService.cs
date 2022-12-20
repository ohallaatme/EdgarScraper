using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;
using FawfulFinance.EdgarBackendScraperAPI.Models.CleanedResults.CompanyFilingModels;
using FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.CompanyFilingModels;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class CompanyFilingsService : ICompanyFilingsService
    {
        private readonly IEdgarDao _edgarDao;

        public CompanyFilingsService(IEdgarDao edgarDao)
        {
            _edgarDao = edgarDao;
        }

        public async Task<CompanyFilingRes> GetCompanyFiling(string cikNumber)
        {
            CompanyFiling rawFiling =  await _edgarDao.GetCompanyFilings(cikNumber);

            return await AggregateDesiredResults(rawFiling);
        }

        public async Task<List<FilingDetail>> GetReportFilings(string cikNumber, string type)
        {
            CompanyFilingRes allData = await GetCompanyFiling(cikNumber);

            return allData.Filings.Where(x => x.Form == type).ToList();

        }

        public async Task<List<string>> GetAvailableFilings(string cikNumber)
        {
            CompanyFiling allData = await _edgarDao.GetCompanyFilings(cikNumber);

            return allData.Filings.Recent.Form.Distinct().ToList();

        }

        private async Task<CompanyFilingRes> AggregateDesiredResults(CompanyFiling companyFiling)
        {
            return await Task.Run(() =>
            {
                CompanyFilingRes res = new CompanyFilingRes
                {
                    Cik = companyFiling.Cik,
                    EntityType = companyFiling.EntityType,
                    Sic = companyFiling.Sic,
                    SicDescription = companyFiling.SicDescription,
                    InsiderTransactionForOwnerExists = companyFiling.InsiderTransactionForOwnerExists,
                    InsiderTransactionForIssuerExists = companyFiling.InsiderTransactionForIssuerExists,
                    Name = companyFiling.Name,
                    Tickers = companyFiling.Tickers,
                    Exchanges = companyFiling.Exchanges,
                    Ein = companyFiling.Ein,
                    Category = companyFiling.Category,
                    FiscalYearEnd = companyFiling.FiscalYearEnd,
                    StateOfIncorporation = companyFiling.StateOfIncorporation,
                    Filings = new List<FilingDetail>()
                };

                if (companyFiling.Filings.Recent.AccessionNumber.Length == 0)
                    return res;

                for (int i=0; i < companyFiling.Filings.Recent.AccessionNumber.Length; i++)
                {
                    FilingDetail detail = new FilingDetail
                    {
                        AccessionNumber = companyFiling.Filings.Recent.AccessionNumber[i],
                        ReportDate = companyFiling.Filings.Recent.ReportDate[i],
                        Form = companyFiling.Filings.Recent.Form[i]
                    };

                    res.Filings.Add(detail);
                }
                return res;
            });
        }
    }
}

