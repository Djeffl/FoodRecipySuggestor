using HtmlAgilityPack;
using SuggestFoodConsole;

Console.WriteLine("Loading data...");

var scraper = new Scraper();

// Leuke recepten
var leukeReceptenData = await scraper.LoadSSRDataAsync("https://www.leukerecepten.nl/recepten");
var lRRecepten = leukeReceptenData.DocumentNode
    .SelectNodes("//div[contains(@class, 'stream-card--list')]/a[@class='full-link']").Select(x => x.GetAttributeValue("href", ""));

Console.WriteLine($"{lRRecepten.Count()} Leuke Recepten recepten...");

// Colruyt 
var colruytData = await scraper.LoadSSRDataAsync("https://www.colruyt.be/nl/lekker-koken/weekmenu");
var colruytRecepten = colruytData.DocumentNode
    .SelectNodes("//a[@class='card card--recipe']").Select(x => x.GetAttributeValue("href", ""));

Console.WriteLine($"{colruytRecepten.Count()} Colruyt recepten...");

// Hello Fresh
var helloFreshGreeceData = await scraper.LoadSSRDataAsync("https://www.hellofresh.be/recipes/griekse-recepten");
var helloFreshGreeceRecepten = helloFreshGreeceData.DocumentNode
    .SelectNodes("//div[@data-test-id='recipe-image-card']/a").Select(x => x.GetAttributeValue("href", ""));

var helloFreshItalyData = await scraper.LoadSSRDataAsync("https://www.hellofresh.be/recipes/italiaanse-recepten");
var helloFreshItalyRecepten = helloFreshItalyData.DocumentNode
    .SelectNodes("//div[@data-test-id='recipe-image-card']/a").Select(x => x.GetAttributeValue("href", ""));

var helloFreshMexicoData = await scraper.LoadSSRDataAsync("https://www.hellofresh.be/recipes/mexicaanse-recepten");
var helloFreshMexicoRecepten = helloFreshMexicoData.DocumentNode
    .SelectNodes("//div[@data-test-id='recipe-image-card']/a").Select(x => x.GetAttributeValue("href", ""));

var helloFreshFrenchData = await scraper.LoadSSRDataAsync("https://www.hellofresh.be/recipes/franse-recepten");
var helloFreshFrenchRecepten = helloFreshFrenchData.DocumentNode
    .SelectNodes("//div[@data-test-id='recipe-image-card']/a").Select(x => x.GetAttributeValue("href", ""));

var helloFreshRecepten = helloFreshGreeceRecepten.Union(helloFreshItalyRecepten).Union(helloFreshMexicoRecepten).Union(helloFreshFrenchRecepten);

Console.WriteLine($"{helloFreshRecepten.Count()} Hello Fresh recepten...");


// Combine
var recepten = colruytRecepten.Union(lRRecepten).Union(helloFreshRecepten);

Console.WriteLine("Press: 'R' to retry");

char keyPress;
do
{
    var suggestion = recepten.Random();
    Console.WriteLine(suggestion);
    keyPress = Console.ReadKey().KeyChar;
    Console.WriteLine();
} while (char.ToUpper(keyPress) == 'R');

Console.WriteLine("The end.");



List<string> ParseHtml(string html)
{
    HtmlDocument htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(html);

    var programmerLinks = htmlDoc.DocumentNode.Descendants("li")
        .Where(node => !node.GetAttributeValue("class", "").Contains("tocsection"))
        .ToList();

    List<string> wikiLink = new List<string>();

    foreach (var link in programmerLinks)
    {
        if (link.FirstChild.Attributes.Count > 0) wikiLink.Add("https://en.wikipedia.org/" + link.FirstChild.Attributes[0].Value);
    }

    return wikiLink;
}