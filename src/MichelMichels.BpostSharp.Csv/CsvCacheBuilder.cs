using MichelMichels.BpostSharp.Models;
using MichelMichels.BpostSharp.Services;

namespace MichelMichels.BpostSharp.Csv;

public class CsvCacheBuilder(string sourceFilePath) : ICacheBuilder<CityData>
{
    private bool isBuilding;

    private readonly List<CityData> cache = [];

    private readonly string sourceFilePath = sourceFilePath ?? throw new ArgumentNullException(nameof(sourceFilePath));

    public Task Clear()
    {
        cache.Clear();
        return Task.CompletedTask;
    }

    public async Task Build()
    {
        if (isBuilding)
        {
            return;
        }

        isBuilding = true;

        await Clear();

        using StreamReader streamReader = new(sourceFilePath);

        while (streamReader.Peek() != -1)
        {
            string? line = await streamReader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            CityData data = ConvertRow(line);

            cache.Add(data);
        }

        isBuilding = false;
    }
    public IEnumerable<CityData> Get() => cache;
    public bool HasCache() => cache.Count != 0;

    private CityData ConvertRow(string line)
    {
        string[] parts = line.Split(',');

        string postalCode = parts[0];
        string cityName = parts[1];

        string mainMunicipality = parts[3];
        string province = parts[4];

        CityData data = new()
        {
            PostalCode = postalCode,
            Name = cityName,
            MainMunicipality = mainMunicipality,
            Province = province,
            IsMunicipality = parts[2] switch
            {
                "Ja" => true,
                "Oui" => true,
                "Neen" => false,
                "Non" => false,
                "" => null,
                _ => null,
            }
        };

        return data;
    }
}
