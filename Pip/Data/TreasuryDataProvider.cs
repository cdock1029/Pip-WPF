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

    public async Task<IEnumerable<Treasury>> GetSavedTreasuriesAsync()
    {
        var treasuries =
            await Task.Run(() => dbContext.Treasuries.AsNoTracking().AsEnumerable());
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

    // 1 call, group by "Type"? https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720
    //or individual:
    //https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720&type=FRN
    public async Task<IEnumerable<Treasury>?> GetAuctionsAsync()
    {
        //type: Bill, Bond, FRN, Note, TIPS, CMB
        var client = GetClient();
        var auctions = await client.GetFromJsonAsync<IEnumerable<Treasury>>(
            "securities/auctioned?format=json&limitByTerm=true&days=720");
        return auctions;
    }


    private HttpClient GetClient()
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(BaseAddress);
        return client;
    }
}
