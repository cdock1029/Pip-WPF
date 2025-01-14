namespace Td.ViewModels;

public class SearchComponentState(ITreasuryDataProvider treasuryDataProvider, InvestmentPageState investmentPageState)
	: BaseStateContainer
{
	public string SearchString
	{
		get;
		set
		{
			if (field == value) return;
			field = value;
			if (string.IsNullOrEmpty(field))
			{
				SelectedOption = null;
				SearchResults = null;
			}

			NotifyStateChanged();
		}
	} = string.Empty;

	public List<TreasuryItemViewModel>? SearchResults
	{
		get;
		private set
		{
			if (field == value) return;
			field = value;
			NotifyStateChanged();
		}
	}

	public TreasuryItemViewModel? SelectedOption
	{
		get;
		set
		{
			if (field == value) return;
			field = value;
			NotifyStateChanged();
		}
	}

	public async Task Search()
	{
		SelectedOption = null;
		SearchResults = [];
		var treasuries = await treasuryDataProvider.SearchTreasuriesAsync(SearchString);
		if (treasuries != null)
			foreach (var t in treasuries)
				SearchResults.Add(new TreasuryItemViewModel
				{
					Cusip = t.Cusip,
					IssueDate = t.IssueDate,
					MaturityDate = t.MaturityDate,
					AuctionDate = t.AuctionDate,
					Term = t.SecurityTerm,
					Type = t.Type
				});
	}

	public void AddInvestmentAndResetList(Investment newInvestment)
	{
		treasuryDataProvider.Add(newInvestment);
		treasuryDataProvider.Save();
		investmentPageState.LoadData();
	}
}