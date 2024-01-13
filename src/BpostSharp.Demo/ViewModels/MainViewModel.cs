using BpostSharp.Models;
using BpostSharp.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BpostSharp.Demo.ViewModels;

public partial class MainViewModel(ICityDataService cityDataService) : ObservableObject
{
    private readonly ICityDataService cityDataService = cityDataService ?? throw new ArgumentNullException(nameof(cityDataService));

    [ObservableProperty]
    private string _searchPostalCode = string.Empty;

    [ObservableProperty]
    private List<CityData> _data = [];

    async partial void OnSearchPostalCodeChanged(string value)
    {
        Data = await cityDataService.GetByPostalCode(value);
    }
}
