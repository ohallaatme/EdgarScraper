
using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;

namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IStatementTypeService
    {
        public FinancialStatementType GetStatementType(string type);
    }
}

