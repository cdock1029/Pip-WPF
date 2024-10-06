using Microsoft.EntityFrameworkCore;
using Pip.DataAccess;
using Pip.Model;
using System.Net.Http;
using System.Net.Http.Json;

namespace Pip.UI.Services;

public class TreasuryDataProvider(HttpClient client, PipDbContext dbContext)
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
		return await client.GetFromJsonAsync<IEnumerable<Treasury>>("securities/upcoming/?format=json")
			.ConfigureAwait(false);
	}

	// 1 call, group by "Type"? https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720
	//or individual:
	//https://www.treasurydirect.gov/TA_WS/securities/auctioned?limitByTerm=true&days=720&type=FRN
	public async Task<IEnumerable<Treasury>?> GetAuctionsAsync()
	{
		//type: Bill, Bond, FRN, Note, TIPS, CMB
		return await client.GetFromJsonAsync<IEnumerable<Treasury>>(
			"securities/auctioned?format=json&limitByTerm=true&days=720").ConfigureAwait(false);
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
