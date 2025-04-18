using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Pip.DataAccess.Services;
using Pip.Web.Client.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

builder.Services.AddSingleton<PreviousAuctionsViewModel>();

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ITreasuryDataProvider, TreasuryClientWebDataProvider>();


await builder.Build().RunAsync();
