using Pip.Model;

namespace Pip.UI.Services;

public interface ITreasuryDataProvider
{
	Task<List<Treasury>> GetSavedAsync();

	Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
	Task<IEnumerable<Treasury>?> GetUpcomingAsync();
	Task<IEnumerable<Treasury>?> GetAuctionsAsync();
	void Add(Treasury treasury);
	Task SaveAsync();
	Task InsertAsync(Treasury treasury);
	Task InsertInvestmentAsync(Investment investment);
	Task<List<Investment>> GetInvestmentsAsync();
	void Add(Investment investment);
	Task DeleteInvestmentsAsync(IEnumerable<Investment> investments);
	Task DeleteTreasuriesAsync(IEnumerable<Treasury> rows);
}
