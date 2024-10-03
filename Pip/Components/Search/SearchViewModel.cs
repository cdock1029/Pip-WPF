using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using Pip.Model;
using Pip.UI.Messages;
using Pip.UI.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ViewModelBase = Pip.UI.ViewModel.ViewModelBase;

namespace Pip.UI.Components.Search;

public partial class SearchViewModel : ViewModelBase
{
	private readonly IMessageBoxService _messageBoxService;

	private readonly ITreasuryDataProvider _treasuryDataProvider;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(SearchCommand))]
	private string? _searchText;

	public SearchViewModel(ITreasuryDataProvider treasuryDataProvider,
		IMessageBoxService messageBoxService)
	{
		_treasuryDataProvider = treasuryDataProvider;
		_messageBoxService = messageBoxService;
		SearchResults.CollectionChanged += SearchResults_CollectionChanged;
	}

	public ObservableCollection<Treasury> SearchResults { get; } = [];

	private void SearchResults_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		ClearResultsCommand.NotifyCanExecuteChanged();
	}


	[RelayCommand(CanExecute = nameof(CanSearch))]
	private async Task Search(string? cusip)
	{
		ArgumentNullException.ThrowIfNull(cusip);

		var treasuries = await _treasuryDataProvider.SearchTreasuriesAsync(cusip.Trim());

		SearchResults.Clear();
		if (treasuries == null) return;
		foreach (var treasury in treasuries)
			SearchResults.Add(treasury);
	}

	private bool CanSearch()
	{
		return !string.IsNullOrWhiteSpace(SearchText);
	}

	[RelayCommand]
	private async Task SaveTreasury(Treasury? treasury)
	{
		ArgumentNullException.ThrowIfNull(treasury);
		var result = _messageBoxService.ShowMessage(
			$"CUSIP: [{treasury.Cusip}]\nIssue Date: [{treasury.IssueDate.ToLongDateString()}]\nMaturity Date: [{treasury.MaturityDate?.ToLongDateString()}]",
			"Do you want to save Treasury?",
			MessageButton.OKCancel,
			MessageIcon.Question
		);

		if (result == MessageResult.OK)
			try
			{
				await _treasuryDataProvider.InsertAsync(treasury);
				WeakReferenceMessenger.Default.Send(
					new AfterTreasuryInsertMessage(new AfterTreasuryInsertArgs(treasury.Cusip, treasury.IssueDate)));
			}
			catch (DbUpdateException e)
			{
				_ = _messageBoxService.ShowMessage(
					$"Error saving UST: {e.Message}\nInner error: {e.InnerException?.Message}", "Error",
					MessageButton.OK, MessageIcon.Error);
			}
	}

	[RelayCommand]
	private async Task CreateInvestment(Treasury? treasury)
	{
		ArgumentNullException.ThrowIfNull(treasury);

		var result = _messageBoxService.ShowMessage(
			$"CUSIP: [{treasury.Cusip}]\nIssue Date: [{treasury.IssueDate.ToLongDateString()}]\nMaturity Date: [{treasury.MaturityDate?.ToLongDateString()}]",
			"Do you want to create investment?",
			MessageButton.OKCancel,
			MessageIcon.Question
		);

		if (result == MessageResult.OK)
			try
			{
				var investment = new Investment
				{
					TreasuryCusip = treasury.Cusip,
					TreasuryIssueDate = treasury.IssueDate,
					Treasury = treasury
				};
				await _treasuryDataProvider.InsertInvestmentAsync(investment);
				WeakReferenceMessenger.Default.Send(
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

	[RelayCommand(CanExecute = nameof(CanClearResults))]
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
