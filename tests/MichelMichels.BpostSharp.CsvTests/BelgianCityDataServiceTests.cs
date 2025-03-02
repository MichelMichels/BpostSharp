using MichelMichels.BpostSharp.Csv;
using MichelMichels.BpostSharp.Models;
using MichelMichels.BpostSharp.Services;
using System.Diagnostics;

namespace MichelMichels.BpostSharp.CsvTests;


[TestClass()]
public sealed class BelgianCityDataServiceTests
{
    private static BelgianCityDataService? belgianCityDataService;

    [TestMethod()]
    public void ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new BelgianCityDataService(null!));
    }

    [TestMethod]
    [DataRow("1000", "Brussel")]
    [DataRow("8000", "Brugge")]
    public async Task SearchCityByPostalCode(string postalCode, string cityName)
    {
        // Arrange

        // Act
        List<CityData> data = await GetService().GetByPostalCode(postalCode);

        // Assert
        Assert.AreEqual(1, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Count(), $"Expected 1, but found following results: [ {string.Join(',', data.Select(x => x.Name))} ]");
        Assert.AreEqual(cityName, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Single().Name);
    }

    [TestMethod]
    [DataRow("Brussel", "1000")]
    [DataRow("Brugge", "8000")]
    public async Task SearchCityDataByName(string cityName, string postalCode)
    {
        // Arrange

        // Act
        List<CityData> data = await GetService().GetByCityName(cityName);

        // Assert
        Assert.AreEqual(1, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Count(), $"Expected 2, but found following results: [ {string.Join(',', data.Select(x => x.Name))} ]");
        Assert.AreEqual(postalCode, data.First().PostalCode);
    }

    [TestMethod]
    public async Task PerformanceTest()
    {
        // Arrange
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act      
        BelgianCityDataService service = GetService();

        // This loops through all possible postal codes, 
        // and there will not be any cache hits as these
        // are all unique.
        for (int i = 0; i < 10000; i++)
        {
            await service.GetByPostalCode(i.ToString());
        }

        // Assert
        Debug.WriteLine($"Elapsed milliseconds: {stopwatch.ElapsedMilliseconds}");
        stopwatch.Stop();
    }

    private static BelgianCityDataService GetService()
    {
        return belgianCityDataService ??= new(new CsvCacheBuilder("Resources/zipcodes_num_nl_2025.csv"));
    }
}

