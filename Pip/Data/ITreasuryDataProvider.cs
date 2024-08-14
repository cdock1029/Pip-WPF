using Pip.Model;

namespace Pip.UI.Data;

public interface ITreasuryDataProvider
{
    Task<IEnumerable<Treasury>> GetSavedAsync();

    Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
    Task<IEnumerable<Treasury>?> GetUpcomingAsync();
    Task<IEnumerable<Treasury>?> GetAuctionsAsync();
    void Add(Treasury treasury);
    Task<int> SaveAsync();
    Task InsertAsync(Treasury treasury);
}
