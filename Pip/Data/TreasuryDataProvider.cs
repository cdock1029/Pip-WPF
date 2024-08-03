using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using Pip.Model;

namespace Pip.UI.Data;

public class TreasuryDataProvider(IHttpClientFactory httpClientFactory) : ITreasuryDataProvider
{
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task<IEnumerable<Treasury>?> GetTreasuries()
    {
        await Task.Delay(100);
        return
        [
            new Treasury
                { Cusip = "ABCDEFG", IssueDate = new DateOnly(2024, 7, 1), MaturityDate = new DateOnly(2024, 8, 1) },
            new Treasury
                { Cusip = "XYZ", IssueDate = new DateOnly(2024, 7, 15), MaturityDate = new DateOnly(2024, 9, 2) },
            new Treasury
                { Cusip = "MNOP", IssueDate = new DateOnly(2024, 8, 7), MaturityDate = new DateOnly(2024, 10, 5) },
            new Treasury
                { Cusip = "QRS", IssueDate = new DateOnly(2025, 1, 1), MaturityDate = new DateOnly(2026, 1, 1) }
        ];
    }

    public async Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip)
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://www.treasurydirect.gov/TA_WS/");

        var treasuries =
            await client.GetFromJsonAsync<IEnumerable<Treasury>>(
                $"securities/search/?format=json&cusip={cusip}");
        return treasuries;
    }
}
