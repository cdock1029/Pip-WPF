using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pip.UI.Data;

namespace Pip.UI.ViewModel;

public partial class TreasuriesViewModel(ITreasuryDataProvider treasuryDataProvider) : ViewModelBase
{
    [ObservableProperty] private TreasuryItemViewModel? _selectedTreasury;

    public ObservableCollection<TreasuryItemViewModel> Treasuries { get; } = [];


    [RelayCommand]
    private void Add(object? parameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter as string);
        //var treasuryViewModel =
        //   new TreasuryItemViewModel(new Treasury { Cusip = "New", SecurityTerm = "", SecurityType = "", Type = "" });
    }

    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;
        var treasuries = await treasuryDataProvider.GetSavedTreasuriesAsync();

        foreach (var treasury in treasuries)
            Treasuries.Add(new TreasuryItemViewModel(treasury));
    }
}
