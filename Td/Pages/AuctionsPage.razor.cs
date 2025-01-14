using Microsoft.Extensions.Caching.Memory;

namespace Td.Pages;

[UsedImplicitly]
public partial class AuctionsPage
{
	private string? _activeTabId = "tab-recent";
	private IEnumerable<TreasuryItemViewModel>? RecentTreasuries { get; set; }
	private IEnumerable<TreasuryItemViewModel>? UpcomingTreasuries { get; set; }

	[Inject] private ITreasuryDataProvider TreasuryDataProvider { get; set; } = null!;
	[Inject] private IMemoryCache Cache { get; set; } = null!;

	[Inject] private IDialogService DialogService { get; set; } = null!;
	[Inject] private NavigationManager Navigation { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		await Task.WhenAll(LoadRecent(), LoadUpcoming());
	}

	private async Task LoadRecent()
	{
		RecentTreasuries = await Cache.GetOrCreateAsync(nameof(RecentTreasuries), async e =>
		{
			e.SetOptions(new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
			});


			var treasuries = await TreasuryDataProvider.GetRecentAsync();
			if (treasuries is null) return [];

			return treasuries.Select(t => new TreasuryItemViewModel
			{
				Cusip = t.Cusip,
				IssueDate = t.IssueDate,
				MaturityDate = t.MaturityDate,
				AuctionDate = t.AuctionDate,
				Term = t.SecurityTerm,
				Type = t.Type
			});
		});
	}

	private async Task LoadUpcoming()
	{
		UpcomingTreasuries = await Cache.GetOrCreateAsync(nameof(UpcomingTreasuries), async e =>
		{
			e.SetOptions(new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
			});

			var treasuries = await TreasuryDataProvider.GetUpcomingAsync();
			if (treasuries is null) return [];
			return treasuries.Select(t => new TreasuryItemViewModel
			{
				Cusip = t.Cusip,
				IssueDate = t.IssueDate,
				MaturityDate = t.MaturityDate,
				AuctionDate = t.AuctionDate,
				Term = t.SecurityTerm,
				Type = t.Type
			});
		});
	}

	private async Task AddInvestment(TreasuryItemViewModel treasuryItem)
	{
		var result = await DialogService.OpenCreateInvestmentDialogAsync(treasuryItem);
		if (result.Data is Investment newInvestment)
		{
			TreasuryDataProvider.Add(newInvestment);
			Navigation.NavigateTo("investments");
		}
	}

	private void NavigateTreasuryItem(TreasuryItemViewModel treasuryItem)
	{
		Navigation.NavigateTo($"treasury/{treasuryItem.Cusip}/{treasuryItem.IssueDate?.ToString("yyyy-MM-dd")}");
	}
}