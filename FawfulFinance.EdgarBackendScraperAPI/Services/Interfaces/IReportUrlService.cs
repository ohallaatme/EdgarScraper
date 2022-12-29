using System;
using FawfulFinance.EdgarBackendScraperAPI.Models.Requests.CoreFinancialStatements;

namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IReportUrlService
    {
        public Task<StatementReqs> GetCoreFinancialStatemenstUrls(string cikNumber, int reportNo);
    }
}   

