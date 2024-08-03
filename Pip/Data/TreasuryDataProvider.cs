using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using Pip.Model;

namespace Pip.UI.Data;

public class TreasuryDataProvider(IHttpClientFactory httpClientFactory) : ITreasuryDataProvider
{
    private const string BaseAddress = "https://www.treasurydirect.gov/TA_WS/";

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task<IEnumerable<Treasury>?> GetTreasuries()
    {
        await Task.Delay(100);
        return
        [
            new Treasury
            {
                Cusip = "912797GL5",
                IssueDate = new DateOnly(2024, 7, 25),
                MaturityDate = new DateOnly(2024, 9, 5),
                SecurityType = "Bill", SecurityTerm = "42-Day"
            },
            new Treasury
            {
                Cusip = "912797KX4",
                IssueDate = new DateOnly(2024, 6, 18),
                MaturityDate = new DateOnly(2024, 8, 13),
                SecurityType = "Bill", SecurityTerm = "8-Week"
            },
            new Treasury
            {
                Cusip = "912797GK7",
                IssueDate = new DateOnly(2024, 5, 9),
                MaturityDate = new DateOnly(2024, 8, 8),
                SecurityType = "Bill", SecurityTerm = "13-Week"
            }
        ];
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
