namespace Td.Data;

public interface ITreasuryService
{
	Task<List<Investment>?> GetInvestmentsAsync();
	List<Investment> GetInvestments();
	Task AddInvestmentAsync(Investment investment);
	void Add(Investment investment);

	Task<List<Treasury>?> SearchTreasuriesAsync(string cusip);
	IAsyncEnumerable<Treasury?> GetUpcomingAsync(CancellationToken ct = default);
	IAsyncEnumerable<Treasury?> GetRecentAsync(CancellationToken ct = default);
	Task<Treasury?> GetTreasuryAsync(string cusip, DateOnly issueDate);
	void Delete(Investment investment);
	Task DeleteInvestmentAsync(Investment investment);
	void Update(Investment investment);
	void SaveChanges();
	Task<IEnumerable<Treasury>?> GetRecentAsyncSimple();
	Task<IEnumerable<Treasury>?> GetUpcomingAsyncSimple();
}