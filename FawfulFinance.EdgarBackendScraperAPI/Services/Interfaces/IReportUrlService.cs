using System;
namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IReportUrlService
    {
        public Task<string> GetFinancialStatements(string cikNumber, int reportNo);
    }
}

