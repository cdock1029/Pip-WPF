using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Pip.Model;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public partial class AuctionsViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
    public ObservableCollection<Treasury> Treasuries { get; } = [];

    [RelayCommand]
    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;

        var auctions = await treasuryDataProvider.GetAuctionsAsync();

        if (auctions is not null)
            foreach (var auction in auctions)
                Treasuries.Add(auction);
    }
}
