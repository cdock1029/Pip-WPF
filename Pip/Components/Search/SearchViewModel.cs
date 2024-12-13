using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using Microsoft.EntityFrameworkCore;
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
			SearchResults.Add(new TreasuryItemViewModel
			{
				Cusip = treasury.Cusip,
				IssueDate = treasury.IssueDate,
				Type = treasury.Type,
				Term = treasury.SecurityTerm
			});
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