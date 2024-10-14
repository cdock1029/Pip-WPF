using System.Diagnostics;
using System.Windows.Threading;
using DevExpress.Mvvm.CodeGenerators;
using Pip.Model;
using Pip.UI.Components.Investments;
using Pip.UI.Services;

namespace Pip.UI.Components.Details;

[GenerateViewModel]
public partial class DetailsViewModel(ITreasuryDataProvider treasuryDataProvider)
{
	private CancellationTokenSource? _tokenSource;
	[GenerateProperty] private Treasury? _treasuryDetailsSelected;

	[GenerateCommand]
	private void HandleSelectionChanged(Treasury? selectedItem)
	{
		TreasuryDetailsSelected = selectedItem;
	}

	[GenerateCommand]
	private void HandleSelectedInvestmentChanged(InvestmentItemViewModel? selected)
	{
		_tokenSource?.Cancel();

		if (selected is null)
		{
			TreasuryDetailsSelected = null;
			return;
		}

		_tokenSource = new CancellationTokenSource();
		try
		{
			var ct = _tokenSource.Token;
			var treasuryTask =
				treasuryDataProvider.LookupTreasuryAsync(selected.Cusip, selected.IssueDate, ct);
			if (treasuryTask.IsCompleted)
				TreasuryDetailsSelected = treasuryTask.Result;
			else
				Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
				{
					var treasury = await treasuryTask;
					if (!ct.IsCancellationRequested) TreasuryDetailsSelected = treasury;
				});
		}
		catch (OperationCanceledException)
		{
			Debug.WriteLine($"Operation canceled for cusip {selected.Cusip}");
		}
	}
}