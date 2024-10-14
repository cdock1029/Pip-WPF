using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pip.DataAccess;
using Pip.Model;

namespace Pip.UI.Services;

public class TreasuryDataProvider(HttpClient client, PipDbContext dbContext, IMemoryCache cache)
	: ITreasuryDataProvider
{
	#region TDApi

	public async Task<IEnumerable<Treasury>?> SearchTreasuriesAsync(string cusip)
	{
		return await client.GetFromJsonAsync<IEnumerable<Treasury>>(
			$"securities/search/?format=json&cusip={cusip}").ConfigureAwait(false);
	}

	public async Task<IEnumerable<Treasury>?> GetUpcomingAsync()
	{
		// TODO: caching here and inside viewmodel is duplicatation. Decide which to use.
		return await cache.GetOrCreateAsync(nameof(GetUpcomingAsync),
				_ => client.GetFromJsonAsync<IEnumerable<Treasury>>("securities/upcoming/?format=json"))
			.ConfigureAwait(false);
	}

	// 1 call, group by "Type"? https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720
	//or individual:
	//https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720&type=FRN
	public async Task<IEnumerable<Treasury>?> GetAuctionsAsync()
	{
		//type: Bill, Bond, FRN, Note, TIPS, CMB
		return await cache.GetOrCreateAsync(nameof(GetAuctionsAsync), _ =>
			client.GetFromJsonAsync<IEnumerable<Treasury>>(
				"securities/auctioned?format=json&limitByTerm=true&days=720")).ConfigureAwait(false);
	}

	public ValueTask<Treasury?> LookupTreasuryAsync(string cusip, DateOnly issueDate, CancellationToken ct)
	{
		var key = (cusip, issueDate);
		var datePath = issueDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
		var path = $"securities/{cusip}/{datePath}";
		// TODO: manage cache size, eviction etc
		var task = cache.GetOrCreateAsync(key, _ => client.GetFromJsonAsync<Treasury>(path, ct));
		return new ValueTask<Treasury?>(task);
	}

	#endregion

	#region DB

	public async Task<List<Investment>> GetInvestmentsAsync()
	{
		var investments = await Task.Run(() => dbContext
			.Investments
			.ToListAsync()).ConfigureAwait(false);
		return investments;
	}

	public async Task InsertInvestmentAsync(Investment investment)
	{
		dbContext.Investments.Add(investment);
		await Task.Run(() => dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}

	public async Task DeleteInvestmentsAsync(IEnumerable<Investment> investments)
	{
		dbContext.Investments.RemoveRange(investments);
		await Task.Run(() => dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}


	public void Add(Investment investment)
	{
		dbContext.Investments.Add(investment);
	}

	public async Task SaveAsync()
	{
		await Task.Run(() => dbContext.SaveChangesAsync()).ConfigureAwait(false);
	}

	#endregion DB
}