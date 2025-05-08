using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pip.DataAccess.Services;
using Pip.Web.Client.Services;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

SyncfusionLicenseProvider.RegisterLicense(
    "MzgyNzI0OUAzMjM5MmUzMDJlMzAzYjMyMzkzYktYZkp3MHg5Y3k4cjMvdi9xVCs0cE1oTiswVENhWVpKczNRUityb05qM1U9");

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMemoryCache();

builder.Services.AddFluentUIComponents();

builder.Services.AddSyncfusionBlazor();

builder.Services.AddSingleton<PreviousAuctionsViewModel>();

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ITreasuryDataProvider, TreasuryClientWebDataProvider>();


await builder.Build().RunAsync();
