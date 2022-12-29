using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;
using FawfulFinance.EdgarBackendScraperAPI.Models.Requests.CoreFinancialStatements;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace FawfulFinance.EdgarBackendScraperAPI.Controllers
{
    [ApiController]
    [Route("Reporting")]
    public class ReportingController : ControllerBase
    {
        private IReportUrlService _reportUrlService;
        private IFinancialReportService _financialReportService;
        private IStatementTypeService _statementTypeService;

        public ReportingController(IReportUrlService reportUrlService,
            IFinancialReportService financialReportService,
            IStatementTypeService statementTypeService)
        {
            _reportUrlService = reportUrlService;
            _financialReportService = financialReportService;
            _statementTypeService = statementTypeService;
        }
        /// <summary>
        /// Get desired core financial statement for company based on CIK number
        /// </summary>
        /// <param name="cikNumber">The ten digit CIK number associated with any stock ticker, includes leading 0s</param>
        /// <param name="reportNo"> Index of report, with 0 being most recent based on recently available filings</param>
        /// <param name="statementType">
        /// cash-flows
        /// balance-sheet
        /// income-statement
        /// stockholders-equity
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetFinancialStatement")]
        public async Task<IActionResult> GetFinancialStatement(string cikNumber, int reportNo, string statementType)
        {
            try
            {
                FinancialStatementType type = _statementTypeService.GetStatementType(statementType);
                string json = await _financialReportService.GetFinancialReport(cikNumber, reportNo, type);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, {ex.InnerException}");
            }
        }


        [HttpGet("GetFinancialUrls")]
        public async Task<IActionResult> GetFinancialUrls(string cikNumber, int reqReport)
        {
            try
            {
                StatementReqs reqs = await _reportUrlService.GetCoreFinancialStatemenstUrls(cikNumber, reqReport);
                return Ok(reqs);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, {ex.InnerException}");
            }
        }
    }
}

