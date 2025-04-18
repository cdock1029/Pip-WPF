using System.Net.Http.Json;
using Pip.DataAccess.Services;
using Pip.Model;

namespace Pip.Web.Client.Services;

public class TreasuryClientWebDataProvider(HttpClient httpClient) : ITreasuryDataProvider
{
    public Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip)
    {
        throw new NotImplementedException();
    }

    private const string DateFormat = "yyyy-MM-dd";

    public Task<IEnumerable<Treasury>?> AnnouncementsResultsSearch(DateOnly startDate, DateOnly endDate)
    {
        return httpClient
            .GetFromJsonAsync<IEnumerable<Treasury>?>(
                $"/api/{nameof(AnnouncementsResultsSearch)}/{startDate.ToString(DateFormat)}/{endDate.ToString(DateFormat)}");
    }

    public Task<IEnumerable<Treasury>?> GetUpcomingAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Treasury>?> GetRecentAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Treasury?> GetRecentAsyncEnumerable()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Treasury?> GetUpcomingAsyncEnumerable()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void Update(Investment investment)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Investment> GetInvestments()
    {
        throw new NotImplementedException();
    }

    public void Add(Investment investment)
    {
        throw new NotImplementedException();
    }

    public void Insert(Investment investment)
    {
        throw new NotImplementedException();
    }

    public void Delete(Investment investment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteInvesmentByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly? issueDate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
