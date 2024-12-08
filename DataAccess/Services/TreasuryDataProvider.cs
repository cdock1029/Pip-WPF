using System.Globalization;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pip.Model;

namespace Pip.DataAccess.Services;

public class TreasuryDataProvider : ITreasuryDataProvider
{
	private readonly IMemoryCache _cache;
	private readonly HttpClient _client;
	private readonly PipDbContext _dbContext;

	public TreasuryDataProvider(HttpClient client, PipDbContext dbContext, IMemoryCache cache)
	{
		client.BaseAddress = new Uri("https://www.treasurydirect.gov/TA_WS/");
		_client = client;
		_dbContext = dbContext;
		_cache = cache;
	}

	#region TDApi

	public async Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip)
	{
		return await _client.GetFromJsonAsync<IEnumerable<Treasury>>(
			$"securities/search/?format=json&cusip={cusip}").ConfigureAwait(false);
	}

	public async Task<IEnumerable<Treasury>?> GetUpcomingAsync()
	{
		// TODO: caching here and inside viewmodel is duplicatation. Decide which to use.
		return await _cache.GetOrCreateAsync(nameof(GetUpcomingAsync),
				_ => _client.GetFromJsonAsync<IEnumerable<Treasury>>("securities/upcoming/?format=json"))
			.ConfigureAwait(false);
	}

	// 1 call, group by "Type"? https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720
	//or individual:
	//https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720&type=FRN
	public async Task<IEnumerable<Treasury>?> GetAuctionsAsync()
	{
		//type: Bill, Bond, FRN, Note, TIPS, CMB
		return await _cache.GetOrCreateAsync(nameof(GetAuctionsAsync), _ =>
			_client.GetFromJsonAsync<IEnumerable<Treasury>>(
				"securities/auctioned?format=json&limitByTerm=true&days=720")).ConfigureAwait(false);
	}

	public ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly issueDate, CancellationToken ct)
	{
		var key = (cusip, issueDate);
		var datePath = issueDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
		var path = $"securities/{cusip}/{datePath}";
		// TODO: manage cache size, eviction etc
		var task = _cache.GetOrCreateAsync(key, _ => _client.GetFromJsonAsync<Treasury>(path, ct));
		return new ValueTask<Treasury?>(task);
	}

	#endregion

	#region DB

	public List<Investment> GetInvestments()
	{
		var investments = _dbContext
			.Investments
			.ToList();
		return investments;
	}

	// TODO: sync I/O faster. https://x.com/davkean/status/1821875521954963742
	public async Task<List<Investment>> GetInvestmentsAsync()
	{
		var investments = await Task.Run(() => _dbContext
			.Investments
			.ToListAsync()).ConfigureAwait(false);
		return investments;
	}

	public async Task InsertInvestmentAsync(Investment investment)
	{
		_dbContext.Investments.Add(investment);
		await Task.Run(() => _dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}

	public async Task DeleteInvestmentsAsync(IEnumerable<Investment> investments)
	{
		_dbContext.Investments.RemoveRange(investments);
		await Task.Run(() => _dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}

	public async Task DeleteInvestmentAsync(Investment investment)
	{
		_dbContext.Investments.Remove(investment);
		await Task.Run(() => _dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}

	public void Add(Investment investment)
	{
		_dbContext.Investments.Add(investment);
	}

	public async Task SaveAsync()
	{
		await Task.Run(() => _dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}

	#endregion DB
}