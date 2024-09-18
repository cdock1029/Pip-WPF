using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Pip.DataAccess;
using Pip.Model;

namespace Pip.UI.Data;

public class TreasuryDataProvider(HttpClient client, PipDbContext dbContext)
	: ITreasuryDataProvider
{
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


	#region DB

	// If using SQLite, the underlying database operations are not async but sync even with async EF calls.
	// Must wrap sync calls inside Task.Run
	public async Task<List<Treasury>> GetSavedAsync()
	{
		return await dbContext.Treasuries.AsNoTracking().ToListAsync().ConfigureAwait(false);
	}

	public List<Treasury> GetSaved()
	{
		return dbContext.Treasuries.AsNoTracking().ToList();
	}

	public async Task<List<Investment>> GetInvestmentsAsync()
	{
		return await dbContext
			.Investments
			.Include(i => i.Treasury)
			.ToListAsync()
			.ConfigureAwait(false);
	}

	public List<Investment> GetInvestments()
	{
		return dbContext.Investments.Include(i => i.Treasury).ToList();
	}

	public Task InsertAsync(Treasury treasury)
	{
		dbContext.Add(treasury);
		return Task.Run(dbContext.SaveChanges);
	}

	public void Add(Treasury treasury)
	{
		dbContext.Treasuries.Add(treasury);
	}

	public void Add(Investment investment)
	{
		dbContext.Investments.Add(investment);
	}

	public Task<int> SaveAsync()
	{
		return Task.Run(dbContext.SaveChanges);
	}

	#endregion DB
}
