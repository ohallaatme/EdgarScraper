using System;
using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Models.CleanedResults.CompanyFilingModels;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IEdgarDao _edgarDao;
        private readonly ICompanyFilingsService _companyFilingsService;

        public ReportingService(IEdgarDao edgarDao, ICompanyFilingsService companyFilingsService)
        {
            _edgarDao = edgarDao;
            _companyFilingsService = companyFilingsService;
        }

        public async Task<string> GetTenKRawReport(string cikNumber, int reportNo)
        {
            // first, get filings
            List<FilingDetail> filings =
                await _companyFilingsService.GetReportFilings(cikNumber, "10-K");

            FilingDetail reqFiling = filings[reportNo];
            string? filingNumber = reqFiling.AccessionNumber;

            if (string.IsNullOrEmpty(filingNumber))
                throw new Exception("The 10K requested does not exist");

            string cikTrimmed = cikNumber.TrimStart(new char[] { '0' });
            return await _edgarDao.GetReportRawText(cikTrimmed, filingNumber);

        }
    }
}

