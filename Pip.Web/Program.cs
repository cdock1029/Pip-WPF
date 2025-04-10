using DevExpress.Blazor;
using Microsoft.EntityFrameworkCore;
using Pip.DataAccess;
using Pip.Web.Components;
using Pip.Web.Components.Pages;
using Pip.Web.Services;
using _Imports = Pip.Web.Client._Imports;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDevExpressBlazor(options =>
{
    //options.BootstrapVersion = BootstrapVersion.v5;
    options.SizeMode = SizeMode.Medium;
});
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<Weather.InvestmentPageState>();

builder.Services.AddDbContextFactory<PipDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection") ??
                         throw new InvalidOperationException("Connection string 'DatabaseConnection' not found."),
        b => b.MigrationsAssembly("Pip.Web"));
});

#if DEBUG

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endif

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();