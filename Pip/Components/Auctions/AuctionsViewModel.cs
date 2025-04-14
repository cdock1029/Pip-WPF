using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using Pip.DataAccess.Services;
using Pip.Model;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments.Messages;
using Pip.UI.Components.Shared;

namespace Pip.UI.Components.Auctions;

[GenerateViewModel]
public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider, DetailsViewModel detailsViewModel)
    : PipViewModel, IPipRoute
{
    [GenerateProperty] private Treasury? _selectedTreasuryRecent;

    [GenerateProperty] private Treasury? _selectedTreasuryUpcoming;

    [GenerateProperty] private IEnumerable<Treasury>? _treasuriesRecent;

    [GenerateProperty] private IEnumerable<Treasury>? _treasuriesUpcoming;

    [GenerateProperty] private bool _isLoading;

    public DetailsViewModel DetailsViewModel => detailsViewModel;

    public string View => nameof(AuctionsView);

    public string Title => "Auctions";

    public Uri Image { get; } =
        DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Sale_Item.svg");

    public override async Task LoadAsync()
    {
        if (TreasuriesRecent != null && TreasuriesRecent.Any() && TreasuriesUpcoming != null &&
            TreasuriesUpcoming.Any()) return;

        await Dispatcher.InvokeAsync(() => IsLoading = true);

        await Task.WhenAll(LoadRecent(), LoadUpcoming());

        await Dispatcher.InvokeAsync(() => IsLoading = false);
    }

    private async Task LoadRecent()
    {
        IEnumerable<Treasury>? recent = await treasuryDataProvider.GetRecentAsync().ConfigureAwait(false);

        await Task.Delay(2000);

        await Dispatcher.InvokeAsync(() => TreasuriesRecent = recent ?? []);
    }

    private async Task LoadUpcoming()
    {
        IEnumerable<Treasury>? upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);

        await Dispatcher.InvokeAsync(() => TreasuriesUpcoming = upcoming ?? []);
    }


    [GenerateCommand]
    private void SaveRecentToInvestments()
    {
        ArgumentNullException.ThrowIfNull(SelectedTreasuryRecent);
        Investment investment = new()
        {
            Cusip = SelectedTreasuryRecent.Cusip,
            IssueDate = SelectedTreasuryRecent.IssueDate!.Value,
            MaturityDate = SelectedTreasuryRecent.MaturityDate,
            SecurityTerm = SelectedTreasuryRecent.SecurityTerm,
            Type = SelectedTreasuryRecent.Type
        };
        treasuryDataProvider.Insert(investment);
        Messenger.Default.Send(new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
    }


    [GenerateCommand]
    private void SaveUpcomingtToInvestments()
    {
        ArgumentNullException.ThrowIfNull(SelectedTreasuryUpcoming);
        Investment investment = new()
        {
            Cusip = SelectedTreasuryUpcoming.Cusip,
            IssueDate = SelectedTreasuryUpcoming.IssueDate!.Value,
            MaturityDate = SelectedTreasuryUpcoming.MaturityDate,
            SecurityTerm = SelectedTreasuryUpcoming.SecurityTerm,
            Type = SelectedTreasuryUpcoming.Type
        };
        treasuryDataProvider.Insert(investment);
        Messenger.Default.Send(new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
    }
}