using PuppeteerSharp;

using ConverterWeb.Services;

namespace ConverterWeb.Converters
{
    public class LocalConverter : IConverter
    {
        private readonly IHtmlConvertedService _convertedService;

        public LocalConverter(IHtmlConvertedService convertedService)
        {
            _convertedService = convertedService;
        }

        public async Task<Guid> SendToConvertAsync(Stream stream)
        {
            var guid = Guid.NewGuid();

            ConvertAsync(stream, guid);

            return guid;
        }

        private async Task ConvertAsync(Stream stream, Guid fileId)
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            string html;
            using (var reader = new StreamReader(stream))
            {
                stream.Position = 0;
                html = await reader.ReadToEndAsync();
            }

            var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);

            await _convertedService.ProcessResultAsync(await page.PdfStreamAsync(), fileId);
        }
    }
}