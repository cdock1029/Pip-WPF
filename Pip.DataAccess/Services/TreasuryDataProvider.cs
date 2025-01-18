using System.Globalization;
using System.Net.Http.Json;
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
		return await _cache.GetOrCreateAsync(nameof(GetUpcomingAsync), e =>
		{
			e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
			return _client.GetFromJsonAsync<IEnumerable<Treasury>>("securities/upcoming/?format=json");
		}).ConfigureAwait(false);
	}

	// 1 call, group by "Type"? https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720
	//or individual:
	//https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720&type=FRN
	public async Task<IEnumerable<Treasury>?> GetRecentAsync()
	{
		//type: Bill, Bond, FRN, Note, TIPS, CMB
		return await _cache.GetOrCreateAsync(nameof(GetRecentAsync), e =>
		{
			e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
			return _client.GetFromJsonAsync<IEnumerable<Treasury>>(
				"securities/auctioned?format=json&limitByTerm=true&days=720");
		}).ConfigureAwait(false);
		;
	}

	public ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly? issueDate, CancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(issueDate);

		var key = (cusip, issueDate);
		var datePath = issueDate.Value.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
		var path = $"securities/{cusip}/{datePath}";
		// TODO: manage cache size, eviction etc
		var task = _cache.GetOrCreateAsync(key, _ => _client.GetFromJsonAsync<Treasury>(path, ct));
		return new ValueTask<Treasury?>(task);
	}

	#endregion

	#region DB

	public void Update(Investment investment)
	{
		if (_dbContext.Investments.Find(investment.Id) is { } inv)
			_dbContext.Entry(inv).CurrentValues.SetValues(investment);
	}

	// sync I/O faster. https://x.com/davkean/status/1821875521954963742
	public IEnumerable<Investment> GetInvestments()
	{
		var investments = _dbContext
			.Investments.AsEnumerable();
		return investments;
	}

	public void Insert(Investment investment)
	{
		_dbContext.Investments.Add(investment);
		_dbContext.SaveChanges();
	}

	public void Delete(Investment investment)
	{
		_dbContext.Investments.Remove(investment);
		_dbContext.SaveChanges();
	}

	public void Add(Investment investment)
	{
		_dbContext.Investments.Add(investment);
	}

	public void Save()
	{
		_dbContext.SaveChanges();
	}

	#endregion DB
}