using AngleSharp;
using AngleSharp.Dom;
using BpostSharp.Models;
using BpostSharp.Services;

namespace BpostSharp.Web;

public class WebCacheBuilder(string endpoint) : ICacheBuilder<CityData>
{
    private readonly string endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

    private readonly List<CityData> cache = [];

    public IEnumerable<CityData> Get() => cache;
    public bool HasCache() => cache.Count != 0;
    public Task Clear()
    {
        cache.Clear();
        return Task.CompletedTask;
    }
    public async Task Build()
    {
        await Clear();

        IConfiguration config = Configuration.Default.WithDefaultLoader();
        IBrowsingContext context = BrowsingContext.New(config);
        IDocument document = await context.OpenAsync(endpoint);
        string rowSelector = "#sheet0 > tbody > tr";
        IHtmlCollection<IElement> rows = document.QuerySelectorAll(rowSelector);

        foreach (IElement row in rows)
        {
            IHtmlCollection<IElement> cells = row.QuerySelectorAll("td");
            if (cells.Length != 5)
            {
                continue;
            }

            cache.Add(ConvertRow(cells));
        }
    }

    private static CityData ConvertRow(IHtmlCollection<IElement> cells)
    {
        return new CityData()
        {
            PostalCode = cells[0].TextContent.Trim(),
            Name = cells[1].TextContent.Trim(),
            MainMunicipality = cells[3].TextContent.Trim(),
            Province = cells[4].TextContent.Trim(),
            IsMunicipality = cells[2].TextContent.Trim() switch
            {
                "Ja" => true,
                "Oui" => true,
                "Neen" => false,
                "Non" => false,
                "" => null,
                _ => null,
            }
        };
    }

}
