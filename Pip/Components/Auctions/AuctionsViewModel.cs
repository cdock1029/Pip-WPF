using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments.Messages;
using Pip.UI.Components.Shared;
using Pip.UI.Data.Services;
using Pip.UI.Models;

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

        await Dispatcher.InvokeAsync(() => TreasuriesRecent = recent ?? []);
    }

    private async Task LoadUpcoming()
    {
        IEnumerable<Treasury>? upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);

        await Dispatcher.InvokeAsync(() => TreasuriesUpcoming = upcoming ?? []);
    }


    [GenerateCommand]
    private async Task SaveRecentToInvestments()
    {
        ArgumentNullException.ThrowIfNull(SelectedTreasuryRecent);
        await SaveToInvestmentsAsync(SelectedTreasuryRecent);
    }


    [GenerateCommand]
    private async Task SaveUpcomingToInvestments()
    {
        ArgumentNullException.ThrowIfNull(SelectedTreasuryUpcoming);
        await SaveToInvestmentsAsync(SelectedTreasuryUpcoming);
    }

    private async Task SaveToInvestmentsAsync(Treasury treasury)
    {
        Investment investment = new()
        {
            Cusip = treasury.Cusip,
            IssueDate = treasury.IssueDate!.Value,
            MaturityDate = treasury.MaturityDate,
            SecurityTerm = treasury.SecurityTerm,
            Type = treasury.Type
        };
        await treasuryDataProvider.InsertAsync(investment);
        Messenger.Default.Send(new AfterInsertInvestmentMessage(new AfterInsertInvestmentArgs(investment.Id)));
    }
}