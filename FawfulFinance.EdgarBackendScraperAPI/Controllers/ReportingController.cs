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

        /// <summary>
        /// Get the raw 10K html for the inputted CIK number
        /// </summary>
        /// <param name="cikNumber">The ten digit CIK number associated with any stock ticker, includes leading 0s</param>
        /// <param name="reqReport">An integer for the report desired, with 0 being the most recent, 1 being second most recent, etc.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("GetTenK")]
        public async Task<IActionResult> GetTenK(string cikNumber, int reqReport)
        {
            try
            {
                string html = await _reportingService.GetTenKRawReport(cikNumber, reqReport);
                return Ok(html);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

