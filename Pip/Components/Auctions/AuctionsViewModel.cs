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

    [GenerateProperty] private IEnumerable<Treasury> _treasuriesRecent = [];

    [GenerateProperty] private IEnumerable<Treasury> _treasuriesUpcoming = [];


    public DetailsViewModel DetailsViewModel => detailsViewModel;

    public string View => nameof(AuctionsView);

    public string Title => "Auctions";

    //public Uri Image { get; } = DXImageHelper.GetImageUri("Images/Business Objects/BOSale_32x32.png");
    public Uri Image { get; } =
        DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Sale_Item.svg");

    public override async Task LoadAsync()
    {
        Task<(IEnumerable<Treasury>?, string)>[] tasks = [LoadRecent(), LoadUpcoming()];


        await foreach (Task<(IEnumerable<Treasury>?, string)> task in Task.WhenEach(tasks).ConfigureAwait(false))
        {
            (IEnumerable<Treasury>? treasuries, string name) = task.Result;
            switch (name)
            {
                case "recent":
                    Dispatcher.BeginInvoke(() => { TreasuriesRecent = treasuries ?? []; });
                    break;
                case "upcoming":
                    Dispatcher.BeginInvoke(() => { TreasuriesUpcoming = treasuries ?? []; });
                    break;
            }
        }
    }

    private async Task<(IEnumerable<Treasury>?, string)> LoadRecent()
    {
        IEnumerable<Treasury>? recent = await treasuryDataProvider.GetRecentAsync().ConfigureAwait(false);
        return (recent, "recent");
    }

    private async Task<(IEnumerable<Treasury>?, string)> LoadUpcoming()
    {
        IEnumerable<Treasury>? upcoming = await treasuryDataProvider.GetUpcomingAsync().ConfigureAwait(false);
        return (upcoming, "upcoming");
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