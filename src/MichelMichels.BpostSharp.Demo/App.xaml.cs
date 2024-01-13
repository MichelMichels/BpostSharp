using MichelMichels.BpostSharp.Demo.Services;
using MichelMichels.BpostSharp.Demo.ViewModels;
using MichelMichels.BpostSharp.Models;
using MichelMichels.BpostSharp.Services;
using MichelMichels.BpostSharp.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace MichelMichels.BpostSharp.Demo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static readonly IHost host = Host
    .CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ApplicationHostService<MainWindow>>();
        services.AddTransient<MainWindow>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<ICacheBuilder<CityData>>(services => new WebCacheBuilder(BpostWebConstants.EndpointDutch));
        services.AddTransient<ICityDataService, BelgianCityDataService>();
    })
    .Build();

    protected override void OnStartup(StartupEventArgs e)
    {
        host.Start();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        host.Dispose();
    }
}

