using Pip.Model;

namespace Pip.DataAccess.Services;

public interface ITreasuryDataProvider
{
	Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
	Task<IEnumerable<Treasury>?> GetUpcomingAsync();
	Task<IEnumerable<Treasury>?> GetAuctionsAsync();
	Task SaveAsync();
	Task InsertInvestmentAsync(Investment investment);
	Task<List<Investment>> GetInvestmentsAsync();
	List<Investment> GetInvestments();
	void Add(Investment investment);
	Task DeleteInvestmentsAsync(IEnumerable<Investment> investments);
	ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly issueDate, CancellationToken ct = default);
}