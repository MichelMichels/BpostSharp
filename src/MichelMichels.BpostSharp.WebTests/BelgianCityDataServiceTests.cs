﻿using MichelMichels.BpostSharp.Models;
using MichelMichels.BpostSharp.Services;
using MichelMichels.BpostSharp.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MichelMichels.BpostSharp.WebTests;

[TestClass()]
public class BelgianCityDataServiceTests
{
    private static BelgianCityDataService? belgianCityDataService;

    [TestMethod()]
    public void ThrowsArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new WebCacheBuilder(null!));
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
        // Act
        List<CityData> data = await GetService().GetByCityName(cityName);

        // Assert
        Assert.IsNotNull(data);
        Assert.AreEqual(1, data.Where(x => x.IsMunicipality.HasValue && !x.IsMunicipality.Value).Count(), $"Expected 2, but found following results: [ {string.Join(',', data.Select(x => x.Name))} ]");
        Assert.AreEqual(postalCode, data.First().PostalCode);
    }

    private static BelgianCityDataService GetService()
    {
        return belgianCityDataService ??= new BelgianCityDataService(new WebCacheBuilder(BpostWebConstants.EndpointDutch));
    }
}