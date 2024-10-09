using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using Microsoft.EntityFrameworkCore;
using Pip.Model;
using Pip.UI.Components.Details;
using Pip.UI.Messages;
using Pip.UI.Services;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Search;

[GenerateViewModel]
public partial class SearchViewModel : PipViewModel
{
	private readonly DetailsViewModel _detailsViewModel;
	private readonly IMessageBoxService _messageBoxService;

	private readonly ITreasuryDataProvider _treasuryDataProvider;

	[GenerateProperty] private string? _searchText;

	public SearchViewModel(ITreasuryDataProvider treasuryDataProvider,
		IMessageBoxService messageBoxService, DetailsViewModel detailsViewModel)
	{
		_treasuryDataProvider = treasuryDataProvider;
		_messageBoxService = messageBoxService;
		_detailsViewModel = detailsViewModel;
		SearchResults.CollectionChanged += SearchResults_CollectionChanged;
	}

	public ObservableCollection<Treasury> SearchResults { get; } = [];

	public Treasury SelectedDetailsTreasury
	{
		set
		{
			_detailsViewModel.TreasuryDetailsSelected = value;
			RaisePropertyChanged();
		}
	}

	private void SearchResults_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ClearResultsCommand.RaiseCanExecuteChanged();
	}

	[GenerateCommand(CanExecuteMethod = nameof(CanSearch))]
	private async Task Search()
	{
		ArgumentNullException.ThrowIfNull(SearchText);

		var treasuries = await _treasuryDataProvider.SearchTreasuriesAsync(SearchText.Trim());

		SearchResults.Clear();
		if (treasuries == null) return;
		foreach (var treasury in treasuries)
			SearchResults.Add(treasury);
	}

	private bool CanSearch()
	{
		return !string.IsNullOrWhiteSpace(SearchText);
	}

	[GenerateCommand]
	private async Task CreateInvestment(Treasury? treasury)
	{
		ArgumentNullException.ThrowIfNull(treasury);

		var result = _messageBoxService.ShowMessage(
			$"CUSIP: [{treasury.Cusip}]\nIssue Date: [{treasury.IssueDate?.ToLongDateString()}]\nMaturity Date: [{treasury.MaturityDate?.ToLongDateString()}]",
			"Do you want to create investment?",
			MessageButton.OKCancel,
			MessageIcon.Question
		);

		if (result == MessageResult.OK)
			try
			{
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
			catch (DbUpdateException e)
			{
				_ = _messageBoxService.ShowMessage(
					$"Error saving Investment: {e.Message}\nInner error: {e.InnerException?.Message}", "Error",
					MessageButton.OK, MessageIcon.Error);
			}
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