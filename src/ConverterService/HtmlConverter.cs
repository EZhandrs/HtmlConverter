using PuppeteerSharp;

namespace ConverterWeb.Converters
{
    public class HtmlConverter
    {
        public async Task<Stream> ConvertAsync(Stream stream)
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

            return await page.PdfStreamAsync();
        }
    }
}