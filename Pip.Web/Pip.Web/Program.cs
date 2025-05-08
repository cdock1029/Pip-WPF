using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using Pip.DataAccess;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.Web.Client.ViewModels;
using Pip.Web.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Syncfusion.Licensing;
using _Imports = Pip.Web.Client._Imports;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["SyncfusionBlazor:Key"]);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddFluentUIComponents();


builder.Services.AddSyncfusionBlazor();

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

builder.Services.AddSingleton<PreviousAuctionsViewModel>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);


RouteGroupBuilder apiGroup = app.MapGroup("/api");

apiGroup.MapGet("auctionsRecent", (ITreasuryDataProvider dataProvider) => dataProvider.GetRecentAsync());

apiGroup.MapGet("auctionsUpcoming", (ITreasuryDataProvider dataProvider) => dataProvider.GetUpcomingAsync());

apiGroup.MapGet($"{nameof(TreasuryDataProvider.AnnouncementsResultsSearch)}/{{start}}/{{end}}",
    (DateOnly start, DateOnly end, ITreasuryDataProvider treasuryDataProvider) =>
        treasuryDataProvider.AnnouncementsResultsSearch(start, end));

apiGroup.MapGet("investments", (ITreasuryDataProvider dataProvider) => dataProvider.GetInvestmentsAsync());

apiGroup.MapPost("investments",
    async (ITreasuryDataProvider dataProvider, [FromBody] DataManagerRequest dataManagerRequest) =>
    {
        // Retrieve data source and convert to queryable.
        IQueryable<Investment> dataSource = (await dataProvider.GetInvestmentsAsync()).AsQueryable();

        if (dataManagerRequest.Sorted is { Count: > 0 })
            dataSource = DataOperations.PerformSorting(dataSource, dataManagerRequest.Sorted);

        // Get total records count.
        int totalRecordsCount = dataSource.Count();

        // Return data and count.
        return new { result = dataSource, count = totalRecordsCount };
    });

apiGroup.MapPost("investments/new",
    async (Investment investment, ITreasuryDataProvider dataProvider, ILogger<Program> logger) =>
    {
        try
        {
            await dataProvider.InsertAsync(investment);
            return Results.Created($"/api/investments/{investment.Id}",
                new { message = "Investment inserted successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting investment");
            return Results.Problem("Error inserting investment");
        }
    });

apiGroup.MapPost("investments/update",
    async ([FromBody] CRUDModel<Investment> updatedRecord, ITreasuryDataProvider dataProvider,
        ILoggerFactory loggerFactory) =>
    {
        ILogger logger = loggerFactory.CreateLogger(nameof(apiGroup));

        try
        {
            if (updatedRecord.Value is { } investment)
            {
                await dataProvider.UpdateAsync(investment);
                logger.LogInformation("Investment {Id} updated", investment.Id);
            }

            return Results.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error inserting investment");

            return Results.Problem("Error inserting investment");
        }
    });

apiGroup.MapPost("investments/delete",
    async ([FromBody] CRUDModel<Investment> deletedRecord, [FromServices] ITreasuryDataProvider dataProvider) =>
    {
        int id = int.Parse(deletedRecord.Key.ToString()!);

        await dataProvider.DeleteInvesmentByIdAsync(id);
    });


app.Run();