using HtmlAgilityPack;
using PuppeteerSharp;

namespace SuggestFoodConsole
{
    internal class Scraper
    {
        private HttpClient _httpClient;
        public Scraper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HtmlDocument> LoadSSRDataAsync(string url)
        {
            var htmlPage = await _httpClient.GetStringAsync(url);

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlPage);

            return htmlDoc;
        }

        public async Task<HtmlDocument> LoadCSRDataAsync(string url)
        {
            var options = new LaunchOptions()
            {
                Headless = true,
                ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                Browser = SupportedBrowser.Chrome
            };

            var browser = await Puppeteer.LaunchAsync(options, null);
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            var htmlPage = await page.GetContentAsync();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlPage);

            return htmlDoc;
        }
    }
}
