using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Pip.Model;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public partial class UpcomingAuctionsViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
    public ObservableCollection<Treasury> Treasuries { get; } = [];

    [RelayCommand]
    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;
        var treasuries = await treasuryDataProvider.GetUpcomingAsync();
        if (treasuries is not null)
            foreach (var treasury in treasuries)
                Treasuries.Add(treasury);
    }
}
