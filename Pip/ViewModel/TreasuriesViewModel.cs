using System.Collections.ObjectModel;
using Pip.Command;
using Pip.Data;
using Pip.Model;

namespace Pip.ViewModel;

public class TreasuriesViewModel : ViewModelBase
{
    private readonly ITreasuryDataProvider _treasuryDataProvider;
    private TreasuryItemViewModel? _selectedTreasury;

    public TreasuriesViewModel(ITreasuryDataProvider treasuryDataProvider)
    {
        _treasuryDataProvider = treasuryDataProvider;
        AddCommand = new DelegateCommand(Add);
    }

    public ObservableCollection<TreasuryItemViewModel> Treasuries { get; } = [];

    public TreasuryItemViewModel? SelectedTreasury
    {
        get => _selectedTreasury;
        set => SetField(ref _selectedTreasury, value);
    }

    public DelegateCommand AddCommand { get; }

    private void Add(object? parameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(parameter as string);
        var treasuryViewModel = new TreasuryItemViewModel(new Treasury { Cusip = "New" });
    }

    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;
        var treasuries = await _treasuryDataProvider.GetTreasuries();
        if (treasuries is not null)
            foreach (var treasury in treasuries)
                Treasuries.Add(new TreasuryItemViewModel(treasury));
    }
}
