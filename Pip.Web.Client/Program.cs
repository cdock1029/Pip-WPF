using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddDevExpressBlazor(options =>
{
    //options.BootstrapVersion = BootstrapVersion.v5;
    options.SizeMode = SizeMode.Medium;
});

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
