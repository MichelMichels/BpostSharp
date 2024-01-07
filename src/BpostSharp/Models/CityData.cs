namespace BpostSharp.Models;

public record CityData
{
    public string PostalCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool? IsMunicipality { get; set; }
    public string MainMunicipality { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
}