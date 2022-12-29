using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class StatementTypeService : IStatementTypeService
    {
        public FinancialStatementType GetStatementType(string type)
        {
            FinancialStatementType stmtType = type switch
            {
                "cash-flows" => FinancialStatementType.CashFlows,
                "balance-sheet" => FinancialStatementType.BalanceSheet,
                "income-statement" => FinancialStatementType.IncomeStatement,
                "stockholders-equity" => FinancialStatementType.StockholdersEquity,
                _ => throw new ArgumentOutOfRangeException(nameof(type), $"{type} is not a valid argument")
            };

            return stmtType;
        }
    }
}

