using Pip.Model;

namespace Pip.UI.Data;

public interface ITreasuryDataProvider
{
	Task<List<Treasury>> GetSavedAsync();

	List<Treasury> GetSaved();

	Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
	Task<IEnumerable<Treasury>?> GetUpcomingAsync();
	Task<IEnumerable<Treasury>?> GetAuctionsAsync();
	void Add(Treasury treasury);
	Task<int> SaveAsync();
	Task InsertAsync(Treasury treasury);
	Task<List<Investment>> GetInvestmentsAsync();
	List<Investment> GetInvestments();
	void Add(Investment investment);
}
