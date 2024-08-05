using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pip.DataAccess;
using Pip.Model;

namespace Pip.UI.Data;

public class TreasuryDataProvider(IHttpClientFactory httpClientFactory, PipDbContext dbContext)
    : ITreasuryDataProvider
{
    private const string BaseAddress = "https://www.treasurydirect.gov/TA_WS/";

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task<IAsyncEnumerable<Treasury>> GetSavedTreasuriesAsync()
    {
        var treasuries =
            await Task.Run(() => dbContext.Treasuries.AsNoTracking().AsAsyncEnumerable());
        return treasuries;
    }


    public async Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip)
    {
        var client = GetClient();
        var treasuries =
            await client.GetFromJsonAsync<IEnumerable<Treasury>>(
                $"securities/search/?format=json&cusip={cusip}");
        return treasuries;
    }

    public async Task<IEnumerable<Treasury>?> GetUpcomingAsync()
    {
        var client = GetClient();

        var treasuries = await client.GetFromJsonAsync<IEnumerable<Treasury>>("securities/upcoming/?format=json");
        return treasuries;
    }

    private HttpClient GetClient()
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseAddress);
        return client;
    }
}
