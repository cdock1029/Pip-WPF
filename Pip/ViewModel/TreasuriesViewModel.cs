using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.UI.Data;
using Pip.UI.Messages;

namespace Pip.UI.ViewModel;

public partial class TreasuriesViewModel(ITreasuryDataProvider treasuryDataProvider)
    : ViewModelBase, IRecipient<AfterTreasuryInsertMessage>
{
    [ObservableProperty] private TreasuryItemViewModel? _selectedTreasury;

    public ObservableCollection<TreasuryItemViewModel> Treasuries { get; } = [];

    public async void Receive(AfterTreasuryInsertMessage message)
    {
        Treasuries.Clear();
        await LoadAsync();
    }


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
        var treasuries = await treasuryDataProvider.GetSavedAsync();

        foreach (var treasury in treasuries)
        {
            Debug.WriteLine($"treasuries from db: {treasury.Cusip}");
            Treasuries.Add(new TreasuryItemViewModel(treasury));
        }
    }
}
