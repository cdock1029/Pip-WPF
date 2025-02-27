using Pip.Model;

namespace Pip.DataAccess.Services;

public interface ITreasuryDataProvider
{
    Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
    Task<IEnumerable<Treasury>?> AnnouncementsResultsSearch(DateOnly startDate, DateOnly endDate);
    Task<IEnumerable<Treasury>?> GetUpcomingAsync();
    Task<IEnumerable<Treasury>?> GetRecentAsync();
    IAsyncEnumerable<Treasury?> GetRecentAsyncEnumerable();
    IAsyncEnumerable<Treasury?> GetUpcomingAsyncEnumerable();
    void Save();
    void Update(Investment investment);
    IEnumerable<Investment> GetInvestments();
    void Add(Investment investment);
    void Insert(Investment investment);
    void Delete(Investment investment);
    Task DeleteInvesmentByIdAsync(int id);
    ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly? issueDate, CancellationToken ct = default);
}