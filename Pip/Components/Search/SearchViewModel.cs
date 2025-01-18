using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.UI.Components.Details;
using Pip.UI.Messages;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Search;

[GenerateViewModel]
public partial class SearchViewModel : PipViewModel
{
	private readonly ITreasuryDataProvider _treasuryDataProvider;

	[GenerateProperty] private bool _hasSearchResults;

	[GenerateProperty] private ObservableCollection<TreasuryItemViewModel> _searchResults = [];
	[GenerateProperty] private TreasuryItemViewModel? _selectedTreasuryItem;

	public SearchViewModel(ITreasuryDataProvider treasuryDataProvider,
		DetailsViewModel detailsViewModel)
	{
		_treasuryDataProvider = treasuryDataProvider;
		DetailsViewModel = detailsViewModel;
		SearchResults.CollectionChanged += (_, _) => { HasSearchResults = SearchResults.Count > 0; };
	}

	public string? SearchText
	{
		get;
		set
		{
			field = value;
			if (string.IsNullOrWhiteSpace(field)) SearchResults.Clear();

			RaisePropertyChanged();
		}
	}

	public DetailsViewModel DetailsViewModel { get; }

	[GenerateCommand]
	private async Task Search()
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(SearchText);

		var treasuries = await _treasuryDataProvider.SearchTreasuriesAsync(SearchText.Trim());

		SearchResults.Clear();
		if (treasuries == null) return;
		foreach (var treasury in treasuries)
			SearchResults.Add(new TreasuryItemViewModel(treasury));
	}

	private bool CanSearch()
	{
		return !string.IsNullOrWhiteSpace(SearchText);
	}

	[GenerateCommand]
	private void CreateInvestment()
	{
		ArgumentNullException.ThrowIfNull(SelectedTreasuryItem);

		var treasury = SelectedTreasuryItem.Treasury;

		var investment = new Investment
		{
			Cusip = treasury.Cusip,
			IssueDate = treasury.IssueDate!.Value,
			MaturityDate = treasury.MaturityDate,
			SecurityTerm = treasury.SecurityTerm,
			Type = treasury.Type
		};
		_treasuryDataProvider.Insert(investment);
		Messenger.Default.Send(
			new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
	}

	private bool CanCreateInvestment()
	{
		return SelectedTreasuryItem is not null;
	}
}