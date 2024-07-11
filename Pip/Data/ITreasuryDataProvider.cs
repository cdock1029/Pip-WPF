using Pip.UI.Model;

namespace Pip.UI.Data;

public interface ITreasuryDataProvider
{
    Task<IEnumerable<Treasury>?> GetTreasuries();
}
