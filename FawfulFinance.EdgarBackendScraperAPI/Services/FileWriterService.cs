using System.Text;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;

namespace FawfulFinance.EdgarBackendScraperAPI.Services
{
    public class FileWriterService : IFileWriterService
    {
        private readonly string _basePath;

        public FileWriterService()
        {
            // TODO: Update to relative path
            _basePath = @"/Users/katherineohalloran/Documents/EdgarApi/EdgarScraper/FawfulFinance.EdgarBackendScraperAPI/Data/Files/{0}";
        }

        public async Task WriteFile(string fileName, string content)
        {
            await Task.Run(() =>
            {
                File.WriteAllText(string.Format(_basePath, fileName), content, Encoding.Unicode);

            });
        }
    }
}

