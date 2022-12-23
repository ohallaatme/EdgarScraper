using System;
namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IReportingService
    {
        public Task GetFinancialStatements(string cikNumber, int reportNo);
    }
}

