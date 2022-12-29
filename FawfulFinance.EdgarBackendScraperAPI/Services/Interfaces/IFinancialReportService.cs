using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;

namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IFinancialReportService
    {
        public Task<string> GetFinancialReport(string cikNumber, int reportNo, FinancialStatementType statementType);
    }
}

