using System;
using System.Text;
using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Models.CleanedResults.CompanyFilingModels;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AngleSharp;
using Newtonsoft.Json;
using FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.FilingUrlModels;
using System.Xml.Linq;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IEdgarDao _edgarDao;
        private readonly ICompanyFilingsService _companyFilingsService;
        private readonly IFileWriterService _fileWriterService;

        public ReportingService(IEdgarDao edgarDao,
            ICompanyFilingsService companyFilingsService,
            IFileWriterService fileWriterService)
        {
            _edgarDao = edgarDao;
            _companyFilingsService = companyFilingsService;
            _fileWriterService = fileWriterService;
        }

        public async Task GetTenKRawReport(string cikNumber, int reportNo)
        {
            // first, get filings
            List<FilingDetail> filings =
                await _companyFilingsService.GetReportFilings(cikNumber, "10-K");

            FilingDetail reqFiling = filings[reportNo];
            string? filingNumber = reqFiling.AccessionNumber;

            if (string.IsNullOrEmpty(filingNumber))
                throw new Exception("The 10K requested does not exist");

            string cikTrimmed = cikNumber.TrimStart(new char[] { '0' });
            string html = await _edgarDao.GetReportRawText(cikTrimmed, filingNumber);

            var config = Configuration.Default;
            using var context = BrowsingContext.New(config);
            using var doc = await context.OpenAsync(req => req.Content(html));

            var rows = doc.QuerySelectorAll("tr");

            StringBuilder sb = new StringBuilder();

            foreach (var r in rows)
            {
                sb.Append(Convert.ToString(r.InnerHtml));
                Console.WriteLine(r.InnerHtml);
            }

            string content = sb.ToString();
            StringBuilder sbName = new StringBuilder(filingNumber);
            sb.Append(".xml");
            string fileName = sbName.ToString();

            await _fileWriterService.WriteFile(fileName, content);
            
        }

        public async Task GetFinancialStatements(string cikNumber, int reportNo)
        {
            List<FilingDetail> filings =
            await _companyFilingsService.GetReportFilings(cikNumber, "10-K");

            FilingDetail reqFiling = filings[reportNo];
            string? filingNumber = reqFiling.AccessionNumber;

            if (string.IsNullOrEmpty(filingNumber))
                throw new Exception("The 10K requested does not exist");

            string cikTrimmed = cikNumber.TrimStart(new char[] { '0' });

            Dictionary<string, string> resp = await _edgarDao.GetMasterReportUrls(cikTrimmed, filingNumber);

            string baseUrl = resp.First().Key;
            string html = resp.First().Value;

            var config = Configuration.Default;
            using var context = BrowsingContext.New(config);
            using var doc = await context.OpenAsync(req => req.Content(html));

            var reportsParent = doc.QuerySelector("myreports");
            var reports = reportsParent.QuerySelectorAll("report");


            List<ReportInfo> reportDocUrls = new List<ReportInfo>();

            for (int i=0; i < reports.Length - 1; i++)
            {
                var outerHtml = reports[i].OuterHtml;

                XDocument xDoc = XDocument.Parse(outerHtml);

                ReportInfo info = new ReportInfo();

                info.NameShort = xDoc.Root.Element("shortname").Value ?? "";
                info.NameLong = xDoc.Root.Element("longname").Value ?? "";
                info.Position = xDoc.Root.Element("position").Value ?? "";
                info.Category = xDoc.Root.Element("menucategory").Value ?? "";

                StringBuilder sbBaseUrl = new StringBuilder(baseUrl);
                string? htmlFileName = xDoc.Root.Element("htmlfilename").Value ?? "";
                sbBaseUrl.Append(htmlFileName);

                info.Url = sbBaseUrl.ToString();

                reportDocUrls.Add(info);
            }


        }
    }
}

