using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Pip.UI.Data.Services;
using Pip.UI.Models;

namespace Pip.UI.Data.Plugins;

[UsedImplicitly]
public sealed class TreasuryPlugin(ITreasuryDataProvider dataProvider, PipDbContext dbContext)
{
	[KernelFunction("list_upcoming_treasury_auctions")]
	[Description(
		"Returns a list of the upcoming US treasury auction securities of various terms and types, i.e 4-week T-Bills, 2-year Notes, 20-year Bonds etc.")]
	[UsedImplicitly]
	public async Task<List<TreasuryData>> ListUpcomingTreasuryAuctions()
	{
		IEnumerable<Treasury>? treasuries = await dataProvider.GetUpcomingAsync();

		return treasuries?.Select(t => new TreasuryData
		{
			Cusip = t.Cusip,
			IssueDate = t.IssueDate,
			AuctionDate = t.AuctionDate,
			Term = t.SecurityTerm,
			Type = t.Type.ToString()
		}).ToList() ?? [];
	}

	[KernelFunction("list_saved_invested_treasuries_in_portfolio")]
	[Description(
		"Returns a list of saved investments of US treasuries in my portfolio. Use this instead of relying on previous messages as my investments may have been updated.")]
	[UsedImplicitly]
	public async Task<List<Investment>> ListSavedInvestedTreasuriesInPortfolio()
	{
		//await using PipDbContext dbContext = await factory.CreateDbContextAsync();

		IEnumerable<Investment> usts = await dbContext.Investments.AsNoTracking().ToListAsync();

		return usts.ToList();
	}

    [KernelFunction("add_treasury_investment_to_portfolio")]
    [Description(
        "Adds an investment to the portfolio for the treasury with given CUSIP and issue date. Par value is optional and defaults to 0 if not passed in.")]
    [UsedImplicitly]
    public async Task AddTreasuryInvestmentToPorfolio(
        [Description("Unique number Treasury Dept. uses to identify securities maturing on a specific date")]
        string cusip,
        [Description(
            "ISO 8601 extended format date-only string (example: 2024-01-31)")]
        string issueDate,
        [Description("The stated $ value of a security on its original issue date, defaults to 0")]
        int parValue = 0)
	{
		DateOnly issueDateOnly = DateOnly.Parse(issueDate.Trim());
		Treasury? t = await dataProvider.LookupTreasuryAsync(cusip, issueDateOnly);
		ArgumentNullException.ThrowIfNull(t);

		//await using PipDbContext dbContext = await factory.CreateDbContextAsync();

		dbContext.Investments.Add(new Investment
		{
			Cusip = cusip,
			IssueDate = issueDateOnly,
			AuctionDate = t.AuctionDate,
			Par = parValue,
			MaturityDate = t.MaturityDate,
			Type = t.Type,
			SecurityTerm = t.SecurityTerm
		});
		await dbContext.SaveChangesAsync();
	}

	[KernelFunction("lookup_treasuries_by_cusip")]
	[Description("Returns a list of treasuries that have the given CUSIP identifier")]
	[UsedImplicitly]
	public async Task<List<TreasuryData>> LookupTreasuriesByCusip(
		[Description("Unique number Treasury Dept. uses to identify securities maturing on a specific date")]
		string cusip)
	{
		IEnumerable<Treasury>? treasuries = await dataProvider.SearchTreasuriesAsync(cusip);
		return treasuries?.Select(t => new TreasuryData
		{
			Cusip = cusip,
			AuctionDate = t.AuctionDate,
			IssueDate = t.IssueDate,
			Type = t.Type.ToString(),
			Term = t.SecurityTerm
		}).ToList() ?? [];
	}

	[KernelFunction("delete_investment_by_id")]
	[Description(
		"Deletes the investment with given id from user's portfolio, returns a boolean signaling success or failure")]
	[UsedImplicitly]
	public async Task<bool> DeleteInvestmentById(int id)
	{
		await dataProvider.DeleteInvesmentByIdAsync(id);
		return true;
	}

	[KernelFunction("get_investments_total")]
	[Description(
		"Total par dollar value of all treasuries invested in my portfolio")]
	[UsedImplicitly]
	public int GetInvestmentsTotal()
	{
		return dataProvider.GetInvestments().Sum(i => i.Par);
	}

	[KernelFunction("update_investment")]
	public async Task<bool> UpdateInvestment([Required] int id,
		[Required] [Description("Only set fields within this type that need to be changed")]
		InvestmentUpdateArgs investmentUpdateArgs)
	{
		ArgumentNullException.ThrowIfNull(investmentUpdateArgs);

		Debug.WriteLine($"update_investment args {investmentUpdateArgs}");

		Investment? investment = await dbContext.Investments.FindAsync(id);

		if (investment is null)
		{
			Debug.WriteLine("investment is null returning false");
			return false;
		}

		Dictionary<string, object> data = [];
		if (investmentUpdateArgs.Par is { } par) data["Par"] = par;
		if (investmentUpdateArgs.Reinvestments is { } reinvestments) data["Reinvestments"] = reinvestments;
		if (investmentUpdateArgs.Confirmation is { } confirmation) data["Confirmation"] = confirmation;

		if (data.Count == 0)
		{
			Debug.WriteLine("No fields to update");
			return false;
		}

		dbContext.Entry(investment).CurrentValues.SetValues(data);

		try
		{
			await dbContext.SaveChangesAsync();
		}
		catch (DbUpdateException ex)
		{
			Debug.WriteLine($"Failed to update investment: {ex.Message}");
			return false;
		}


		return true;
	}


	[Description("A US government issued treasury security")]
	public sealed class TreasuryData
	{
		[Description("Nine digit identifier for a treasury security")]
		public string Cusip { [UsedImplicitly] get; set; } = null!;

		[Description("Date when treasury will be sold at auction")]
		public DateOnly? AuctionDate { [UsedImplicitly] get; set; }

		[Description("Date when treasury will be issued to buyers")]
		public DateOnly? IssueDate { [UsedImplicitly] get; set; }

		[Description("The length of time the security earns interest, from issue date to maturity date")]
		public string? Term { [UsedImplicitly] get; set; }

		[Description("Which type of treasury: Bill, Note, Bond, CMB, TIPS, or FRN")]
		public string? Type { [UsedImplicitly] get; set; }
	}

	[UsedImplicitly]
	public sealed record InvestmentUpdateArgs(
		[Optional] int? Par,
		[Optional] int? Reinvestments,
		[Optional] string? Confirmation);
}