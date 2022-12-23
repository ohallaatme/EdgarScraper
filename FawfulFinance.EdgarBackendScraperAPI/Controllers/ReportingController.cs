using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace FawfulFinance.EdgarBackendScraperAPI.Controllers
{
    [ApiController]
    [Route("Reporting")]
    public class ReportingController : ControllerBase
    {
        private IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }   

        [HttpGet("GetFinancials")]
        public async Task<IActionResult> GetFinancials(string cikNumber, int reqReport)
        {
            try
            {
                await _reportingService.GetFinancialStatements(cikNumber, reqReport);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, {ex.InnerException}");
            }
        }
    }
}

