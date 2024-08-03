using System.Collections.ObjectModel;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public class UpcomingAuctionsViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
    public ObservableCollection<TreasuryItemViewModel> Treasuries { get; } = [];

    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;

        var treasuries = await treasuryDataProvider.GetUpcomingAsync();
        if (treasuries is not null)
            foreach (var treasury in treasuries)
                Treasuries.Add(new TreasuryItemViewModel(treasury));
    }
}
