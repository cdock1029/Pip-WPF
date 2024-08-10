using Pip.Model;

namespace Pip.UI.Data;

public interface ITreasuryDataProvider
{
    Task<IEnumerable<Treasury>> GetSavedTreasuriesAsync();

    Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
    Task<IEnumerable<Treasury>?> GetUpcomingAsync();
    Task<IEnumerable<Treasury>?> GetAuctionsAsync();
}
