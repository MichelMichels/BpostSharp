using BpostSharp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BpostSharp.Excel.Tests;

[TestClass()]
public class BelgianCityDataServiceTests
{
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
        BelgianCityDataService belgianCityDataService = new("Resources/zipcodes_num_nl_new.xls");

        // Act
        List<CityData> data = await belgianCityDataService.GetByPostalCode(postalCode);

        // Assert
        Assert.IsNotNull(data);
        Assert.AreEqual(1, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Count(), $"Expected 1, but found following results: [ {string.Join(',', data.Select(x => x.Name))} ]");
        Assert.AreEqual(cityName, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Single().Name);
    }

    [TestMethod]
    [DataRow("Brussel", "1000")]
    [DataRow("Brugge", "8000")]
    public async Task SearchCityDataByName(string cityName, string postalCode)
    {
        // Arrange
        BelgianCityDataService belgianCityDataService = new("Resources/zipcodes_num_nl_new.xls");

        // Act
        List<CityData> data = await belgianCityDataService.GetByCityName(cityName);

        // Assert
        Assert.IsNotNull(data);
        Assert.AreEqual(1, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Count(), $"Expected 2, but found following results: [ {string.Join(',', data.Select(x => x.Name))} ]");
        Assert.AreEqual(postalCode, data.First().PostalCode);
    }
}