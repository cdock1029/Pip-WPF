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
	private readonly IMessageBoxService _messageBoxService;
	private readonly ITreasuryDataProvider _treasuryDataProvider;

	[GenerateProperty] private ObservableCollection<TreasuryItemViewModel> _searchResults = [];
	[GenerateProperty] private TreasuryItemViewModel? _selectedTreasuryItem;

	public SearchViewModel(ITreasuryDataProvider treasuryDataProvider,
		IMessageBoxService messageBoxService, DetailsViewModel detailsViewModel)
	{
		_treasuryDataProvider = treasuryDataProvider;
		_messageBoxService = messageBoxService;
		DetailsViewModel = detailsViewModel;
		SearchResults.CollectionChanged += (_, _) =>
			ClearResultsCommand.RaiseCanExecuteChanged();
	}

	public string? SearchText
	{
		get;
		set
		{
			field = value;
			if (string.IsNullOrWhiteSpace(field))
			{
				SearchResults.Clear();
				SearchResults = [];
			}

			RaisePropertyChanged();
		}
	}

	public DetailsViewModel DetailsViewModel { get; }

	[GenerateCommand]
	private async Task Search()
	{
		ArgumentNullException.ThrowIfNull(SearchText);

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
	private async Task CreateInvestment()
	{
		ArgumentNullException.ThrowIfNull(SelectedTreasuryItem);

		var treasury = SelectedTreasuryItem.Treasury;

		var investment = new Investment
		{
			Cusip = treasury.Cusip,
			IssueDate = treasury.IssueDate ?? default,
			MaturityDate = treasury.MaturityDate,
			SecurityTerm = treasury.SecurityTerm,
			Type = treasury.Type
		};
		await _treasuryDataProvider.InsertInvestmentAsync(investment);
		Messenger.Default.Send(
			new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id, treasury.Cusip,
				treasury.IssueDate)));
	}

	private bool CanCreateInvestment()
	{
		return SelectedTreasuryItem is not null;
	}

	[GenerateCommand]
	private void ClearResults()
	{
		SearchResults.Clear();
		SearchText = string.Empty;
	}

	private bool CanClearResults()
	{
		return SearchResults.Count > 0;
	}
}