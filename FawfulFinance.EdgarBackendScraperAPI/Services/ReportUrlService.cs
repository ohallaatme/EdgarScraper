using System.Text;
using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Models.CleanedResults.CompanyFilingModels;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using AngleSharp;
using FawfulFinance.EdgarBackendScraperAPI.Models.RawResponses.FilingUrlModels;
using System.Xml.Linq;
using FawfulFinance.EdgarBackendScraperAPI.Models.Requests.CoreFinancialStatements;
using Newtonsoft.Json;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class ReportUrlService : IReportUrlService
    {
        private readonly IEdgarDao _edgarDao;
        private readonly ICompanyFilingsService _companyFilingsService;

        public ReportUrlService(IEdgarDao edgarDao,
            ICompanyFilingsService companyFilingsService)
        {
            _edgarDao = edgarDao;
            _companyFilingsService = companyFilingsService;
        }

        public async Task<StatementReqs> GetCoreFinancialStatemenstUrls(string cikNumber, int reportNo)
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

            StatementReqs reqs = await GetCoreFinancialReportUrls(reportDocUrls);
            return reqs;
        }

        private static async Task<StatementReqs> GetCoreFinancialReportUrls(List<ReportInfo> fullInfo)
        {

            if (fullInfo.Count == 0)
                throw new Exception("No reports available for this Company");

            StatementReqs res = new StatementReqs();

            // Statements category should be consistent across companies, but keep an eye on
            // adding Uncategorized from Amazon example
            List<ReportInfo> statements = fullInfo
                .Where(r => r.Category == "Statements" || r.Category == "Uncategorized")
                .ToList();

            await GetCashFlowsUrl(res, statements);
            await GetBalanceSheetUrl(res, statements);
            await GetIncomeStatementUrl(res, statements);
            await GetStockholdersEquity(res, statements);

            return res;
        }

        private static async Task GetCashFlowsUrl(StatementReqs res, List<ReportInfo> statements)
        {
            List<ReportInfo> potentialCashFlow = statements.Where(r => r.NameShort
                .ToLower()
                .Contains("cash")).ToList();

            if (potentialCashFlow.Count == 1)
                res.CashFlowsUrl = potentialCashFlow.First().Url;
            else if (potentialCashFlow.Count == 0)
                res.CashFlowsUrl = "";
            else
                res.CashFlowsUrl = await GetMostApplicableUrl(potentialCashFlow, "cash flows");

        }

        private static async Task GetBalanceSheetUrl(StatementReqs res, List<ReportInfo> statements)
        {
            List<ReportInfo> potentialBalanceSheets = statements.Where(r => r.NameShort
                .ToLower()
                .Contains("balance")).ToList();

            if (potentialBalanceSheets.Count == 1)
                res.BalanceSheetUrl = potentialBalanceSheets.First().Url;
            else if (potentialBalanceSheets.Count == 0)
                res.BalanceSheetUrl = "";
            else
                res.BalanceSheetUrl = await GetMostApplicableUrl(potentialBalanceSheets, "balance sheet");  
        }

        private static async Task GetIncomeStatementUrl(StatementReqs res, List<ReportInfo> statements)
        {
            List<ReportInfo> potentialIncomeStatement = statements.Where(r => r.NameShort
                .ToLower()
                .Contains("income")).ToList();


            if (potentialIncomeStatement.Count == 1)
                res.IncomeStatementUrl = potentialIncomeStatement.First().Url;
            else if (potentialIncomeStatement.Count == 0)
                res.IncomeStatementUrl = "";
            else
                res.IncomeStatementUrl = await GetMostApplicableUrl(potentialIncomeStatement, "income statement");
        }

        private static async Task GetStockholdersEquity(StatementReqs res, List<ReportInfo> statements)
        {
            List<ReportInfo> potentialStockholdersEquity = statements.Where(r => r.NameShort
                .ToLower()
                .Contains("stockholder")).ToList();

            if (potentialStockholdersEquity.Count == 1)
                res.StockholdersEquityUrl = potentialStockholdersEquity.First().Url;
            else if (potentialStockholdersEquity.Count == 0)
                res.StockholdersEquityUrl = "";
            else
                res.StockholdersEquityUrl = await GetMostApplicableUrl(potentialStockholdersEquity, "stockholders equity");
        }

        private static async Task<string> GetMostApplicableUrl(List<ReportInfo> potentialMatches, string comparison)
        {

            Dictionary<string, int> levRes = new Dictionary<string, int>();
            foreach (ReportInfo r in potentialMatches)
            {
                int distance = await ComputeLevenshteinDistance(r.NameShort.ToLower(), comparison);
                levRes.Add(r.Url, distance);
            }

            int minVal = levRes.Values.Min();

            return levRes.FirstOrDefault(x => x.Value == minVal).Key;

        }

        private static async Task<int> ComputeLevenshteinDistance(string stringOne, string stringTwo)
        {
            return await Task.Run(() =>
            {
                // n
                int sOneLength = stringOne.Length;

                // m
                int sTwoLength = stringTwo.Length;

                // 2D arrays require fewer allocations upon the managed heap and may be faster in this context
                int[,] distance = new int[sOneLength + 1, sTwoLength + 1];

                // guard clauses
                if (sOneLength == 0)
                    return sTwoLength;

                if (sTwoLength == 0)
                    return sOneLength;

                // initialize arrays
                for (int i = 0; i <= sOneLength; distance[i, 0] = i++)
                { }
                for (int j = 0; j <= sTwoLength; distance[0, j] = j++)
                { }

                for (int i = 1; i <= sOneLength; i++)
                {
                    for (int j=1; j <= sTwoLength; j++)
                    {
                        // compute cost
                        int cost = (stringTwo[j - 1] == stringOne[i - 1]) ? 0 : 1;
                        distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1,
                        distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                    }
                }
                return distance[sOneLength, sTwoLength];
            });
        }
    }
}

