namespace FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces
{
    public interface IFileWriterService
    {
        public Task WriteFile(string fileName, string content);
    }
}

