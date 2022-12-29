using System.Xml.Linq;
using AngleSharp;
using FawfulFinance.EdgarBackendScraperAPI.Data.Enums;
using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Models.Requests.CoreFinancialStatements;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;
using Newtonsoft.Json;
using System.Data;
using AngleSharp.Dom;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class FinancialReportService : IFinancialReportService
    {
        private readonly IReportUrlService _reportUrlService;
        private readonly IEdgarDao _edgarDao;

        public FinancialReportService(IReportUrlService reportUrlService,
            IEdgarDao edgarDao)
        {
            _reportUrlService = reportUrlService;
            _edgarDao = edgarDao;
        }

        public async Task<string> GetFinancialReport(string cikNumber, int reportNo, FinancialStatementType statementType)
        {
            StatementReqs reqs =
                await _reportUrlService.GetCoreFinancialStatemenstUrls(cikNumber, reportNo);

            string url = statementType switch
            {
                FinancialStatementType.CashFlows => reqs.CashFlowsUrl,
                FinancialStatementType.BalanceSheet => reqs.BalanceSheetUrl,
                FinancialStatementType.IncomeStatement => reqs.IncomeStatementUrl,
                FinancialStatementType.StockholdersEquity => reqs.StockholdersEquityUrl,
                _ => throw new ArgumentOutOfRangeException(nameof(statementType))
            };

            DataTable tbl = await GetFinancialTable(url);

            return JsonConvert.SerializeObject(tbl, Formatting.Indented);

        }


        private async Task<DataTable> GetFinancialTable(string url)
        {
            string html = await _edgarDao.GetFinancialStatement(url);

            var config = Configuration.Default;
            using var context = BrowsingContext.New(config);
            using var doc = await context.OpenAsync(req => req.Content(html));

            var tableHeaders = doc.QuerySelectorAll("th");
            var firstTable = doc.QuerySelector("table");
            var tableVals = firstTable.QuerySelectorAll("td");

            DataTable res = new DataTable();

            await AddTableCols(tableHeaders, res);
            await PopulateTableValues(tableVals, res);

            return res;
        }

        private async Task AddTableCols(IHtmlCollection<IElement> tableHeaders, DataTable res)
        {
            await Task.Run(() =>
            {
                for (int i=0; i < tableHeaders.Length; i++)
                {
                    var innerHtml = tableHeaders[i].OuterHtml;
                    innerHtml = innerHtml.Replace("<br>", "");

                    XDocument xDoc = XDocument.Parse(innerHtml);

                    string? colName;

                    if (innerHtml.Contains("<strong>"))
                    {
                        colName = xDoc.Root.Element("div").Element("strong").Value ?? "";
                        res.Columns.Add(colName);
                    }
                    else if (innerHtml.Contains("colspan"))
                    {
                        var colSpan = xDoc.Root.Attribute("colspan").Value ?? "";

                        int? colSpanVal = Convert.ToInt32(colSpan);

                        if (colSpanVal == 1)
                        {
                            colName = xDoc.Root.Element("th").Value ?? "";
                            res.Columns.Add(colName);
                        }
                    }
                    else
                    {
                        colName = xDoc.Root.Element("div").Value ?? "";
                        res.Columns.Add(colName);
                    }
                }
            });
        }

        private async Task PopulateTableValues(IHtmlCollection<IElement> tableVals, DataTable res)
        {
            await Task.Run(() =>
            {
                int leftPointer = 0;
                int rightPointer = res.Columns.Count - 1;

                while (rightPointer < tableVals.Length)
                {
                    DataRow row = res.NewRow();
                    IElement[] rowVals = tableVals.Skip(leftPointer).Take(res.Columns.Count).ToArray();
                    for (int i=0; i < rowVals.Length; i++)
                    {
                        var outerHtml = rowVals[i].OuterHtml;
                        string? val;

                        if (outerHtml.Contains("&nbsp"))
                        {   
                            val = "";
                            row[i] = val;
                            // avoid xml parser error for empty values with the &nsbp value
                            continue;
                        }


                        XDocument xDoc = XDocument.Parse(outerHtml);

                        if (outerHtml.Contains("strong"))
                        {
                            val = xDoc.Root.Element("a").Element("strong").Value ?? "";
                        }
                        // nump is the name of the value class with the #s
                        else if (outerHtml.Contains("num"))
                        {
                            val = xDoc.Root.Value ?? "";
                        }
                        else if (outerHtml.Contains("text"))
                        {
                            // blank value
                            val = "";
                        }
                        else
                        {
                            val = xDoc.Root.Element("a").Value ?? "";
                        }

                        row[i] = val;
                    }

                    res.Rows.Add(row);
                    leftPointer = rightPointer + 1;
                    rightPointer += res.Columns.Count;
                }
            });

        }
    }
}














