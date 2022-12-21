using System;
namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IReportingService
    {
        public Task<string> GetTenKRawReport(string cikNumber, int reportNo);
    }
}

