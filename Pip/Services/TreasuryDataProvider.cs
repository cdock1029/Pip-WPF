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

	// If using SQLite, the underlying database operations are not async but sync even with async EF calls.
	// Must wrap sync calls inside Task.Run
	public async Task<List<Treasury>> GetSavedAsync()
	{
		var treasuries = await Task.Run(() => dbContext.Treasuries.ToList());
		return treasuries;
	}

	public async Task<List<Investment>> GetInvestmentsAsync()
	{
		var investments = await Task.Run(() => dbContext
			.Investments
			.Include(i => i.Treasury)
			.ToList());
		return investments;
	}

	public async Task InsertAsync(Treasury treasury)
	{
		dbContext.Add(treasury);
		await Task.Run(dbContext.SaveChanges);
	}

	public async Task InsertInvestmentAsync(Investment investment)
	{
		var treasury = await Task.Run(() => dbContext.Treasuries.FirstOrDefault(t =>
			t.Cusip == investment.Treasury.Cusip && t.IssueDate == investment.Treasury.IssueDate));

		if (treasury is null)
			dbContext.Entry(investment.Treasury).State = EntityState.Added;
		else
			investment.Treasury = treasury;

		dbContext.Investments.Add(investment);
		await Task.Run(dbContext.SaveChanges);
	}

	public async Task DeleteInvestmentsAsync(IEnumerable<Investment> investments)
	{
		dbContext.Investments.RemoveRange(investments);
		await Task.Run(dbContext.SaveChanges);
	}

	public async Task DeleteTreasuriesAsync(IEnumerable<Treasury> rows)
	{
		dbContext.Treasuries.RemoveRange(rows);
		await Task.Run(dbContext.SaveChanges);
	}

	public void Add(Treasury treasury)
	{
		dbContext.Treasuries.Add(treasury);
	}

	public void Add(Investment investment)
	{
		dbContext.Investments.Add(investment);
	}

	public async Task SaveAsync()
	{
		await Task.Run(dbContext.SaveChanges);
	}

	#endregion DB
}
