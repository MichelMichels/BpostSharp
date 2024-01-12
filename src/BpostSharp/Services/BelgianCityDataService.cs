using BpostSharp.Models;
using BpostSharp.Services;

namespace BpostSharp.Excel;

public class BelgianCityDataService(ICacheBuilder<CityData> cacheBuilder) : ICityDataService
{
    private readonly ICacheBuilder<CityData> cacheBuilder = cacheBuilder ?? throw new ArgumentNullException(nameof(cacheBuilder));

    public async Task<List<CityData>> GetByCityName(string name)
    {
        if (!cacheBuilder.HasCache())
        {
            await cacheBuilder.Build();
        }

        return cacheBuilder.Get()
            .Where(x => x.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
    }

    public async Task<List<CityData>> GetByPostalCode(string postalCode)
    {
        if (!cacheBuilder.HasCache())
        {
            await cacheBuilder.Build();
        }

        return cacheBuilder.Get()
            .Where(x => x.PostalCode.StartsWith(postalCode))
            .ToList();
    }
}
