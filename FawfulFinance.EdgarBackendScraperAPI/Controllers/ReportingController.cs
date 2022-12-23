using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace FawfulFinance.EdgarBackendScraperAPI.Controllers
{
    [ApiController]
    [Route("Reporting")]
    public class ReportingController : ControllerBase
    {
        private IReportUrlService _reportingService;

        public ReportingController(IReportUrlService reportingService)
        {
            _reportingService = reportingService;
        }   

        [HttpGet("GetFinancials")]
        public async Task<IActionResult> GetFinancials(string cikNumber, int reqReport)
        {
            try
            {
                string json = await _reportingService.GetFinancialStatements(cikNumber, reqReport);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, {ex.InnerException}");
            }
        }
    }
}

