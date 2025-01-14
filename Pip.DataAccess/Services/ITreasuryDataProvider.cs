using Pip.Model;

namespace Pip.DataAccess.Services;

public interface ITreasuryDataProvider
{
	Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip);
	Task<IEnumerable<Treasury>?> GetUpcomingAsync();
	Task<IEnumerable<Treasury>?> GetAuctionsAsync();
	void Save();
	void Update(Investment investment);
	List<Investment> GetInvestments();
	void Add(Investment investment);
	void Insert(Investment investment);
	void DeleteRange(IEnumerable<Investment> investments);
	void Delete(Investment investment);
	ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly? issueDate, CancellationToken ct = default);
}