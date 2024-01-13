using MichelMichels.BpostSharp.Models;

namespace MichelMichels.BpostSharp.Services;

public interface ICityDataService
{
    Task<List<CityData>> GetByPostalCode(string postalCode);
    Task<List<CityData>> GetByCityName(string name);
}
