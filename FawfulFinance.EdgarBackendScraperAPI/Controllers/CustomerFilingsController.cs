using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FawfulFinance.EdgarBackendScraperAPI.Controllers;

[ApiController]
[Route("CustomerFilings")]
public class CustomerfilingsController : ControllerBase
{
    private ICompanyFilingsService _companyFilingsService;
    public CustomerfilingsController(ICompanyFilingsService companyFilingsService)
    {
        _companyFilingsService = companyFilingsService;
    }

    /// <summary>
    /// Get all filings for specific company
    /// </summary>
    /// <param name="cikNumber">The ten digit CIK number associated with any stock ticker, includes leading 0s</param>
    /// <returns></returns>
    /// <exception cref="Exception">Upon error, will return the exception message</exception>
    [HttpGet("GetSingleCustomerFiling")]
    public async Task<IActionResult> GetSingleCustomerFiling(string cikNumber)
    {
        try
        {
            var rawRes = await _companyFilingsService.GetCompanyFiling(cikNumber);
            return Ok(rawRes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    /// <summary>
    /// Get report filing numbers for specific company and report type
    /// </summary>
    /// <param name="cikNumber">The ten digit CIK number associated with any stock ticker, includes leading 0s</param>
    /// <param name="type">The string of the report type that matches the EDGAR format - i.e. 10-K for a 10K</param>
    /// <returns></returns>
    /// <exception cref="Exception">Upon error, will return the exception message</exception>
    [HttpGet("GetReportFilings")]
    public async Task<IActionResult> GetAnnualReportFilings(string cikNumber, string type)
    {
        try
        {
            var rawRes = await _companyFilingsService.GetReportFilings(cikNumber, type);
            return Ok(rawRes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    /// Get a distinct list of the available reports for a single company
    /// </summary>
    /// <param name="cikNumber">The ten digit CIK number associated with any stock ticker, includes leading 0s</param>
    /// <returns></returns>
    [HttpGet("GetAvailableFilings")]
    public async Task<IActionResult> GetAvailableFilings(string cikNumber)
    {
        try
        {
            var rawRes = await _companyFilingsService.GetAvailableFilings(cikNumber);
            return Ok(rawRes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}

