using System.Collections.ObjectModel;
using System.Windows.Data;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public class AuctionsViewModel : ViewModelBase
{
    private readonly ITreasuryDataProvider _treasuryDataProvider;

    public AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider)
    {
        _treasuryDataProvider = treasuryDataProvider;
        GroupedTreasuries = new CollectionViewSource { Source = Treasuries };
        GroupedTreasuries.GroupDescriptions.Add(new PropertyGroupDescription(nameof(TreasuryItemViewModel.Type)));
    }

    public CollectionViewSource GroupedTreasuries { get; }

    private ObservableCollection<TreasuryItemViewModel> Treasuries { get; } = [];

    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;

        var auctions = await _treasuryDataProvider.GetAuctionsAsync();

        if (auctions is not null)
            foreach (var auction in auctions)
                Treasuries.Add(new TreasuryItemViewModel(auction));
    }
}
