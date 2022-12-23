using System;
namespace FawfulFinance.EdgarBackendScraperAPI.Models.Requests.CoreFinancialStatements
{
    public class StatementReqs
    {
        // TODO: potentially include CIK number or ticker
        public string BalanceSheetUrl { get; set; }
        public string IncomeStatementUrl { get; set; }
        public string CashFlowsUrl { get; set; }
        public string StockholdersEquityUrl { get; set; }
    }
}

