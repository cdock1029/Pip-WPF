using DevExpress.Blazor;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.Web.Components;
using Pip.Web.Services;
using _Imports = Pip.Web.Client._Imports;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

builder.Services.AddMvc();

builder.Services.AddMemoryCache();
builder.Services
    .AddHttpClient<ITreasuryDataProvider, TreasuryDataProvider>();

builder.Services.AddDbContextFactory<PipDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection") ??
                         throw new InvalidOperationException("Connection string 'DatabaseConnection' not found."),
        b => b.MigrationsAssembly("Pip.Web"));
});

builder.Services.AddSingleton<AppState>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();
