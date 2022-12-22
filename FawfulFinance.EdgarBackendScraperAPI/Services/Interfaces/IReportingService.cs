using System;
namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IReportingService
    {
        public Task GetTenKRawReport(string cikNumber, int reportNo);

        public Task GetFinancialStatements(string cikNumber, int reportNo);
    }
}

