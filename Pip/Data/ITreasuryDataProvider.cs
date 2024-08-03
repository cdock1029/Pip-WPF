using Pip.Model;

namespace Pip.UI.Data;

public interface ITreasuryDataProvider
{
    Task<IEnumerable<Treasury>?> GetTreasuries();

    Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
    Task<IEnumerable<Treasury>?> GetUpcomingAsync();
}
