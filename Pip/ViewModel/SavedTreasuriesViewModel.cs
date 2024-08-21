using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pip.Model;
using Pip.UI.Data;
using Pip.UI.Messages;
using Pip.UI.View.Services;

namespace Pip.UI.ViewModel;

public partial class SavedTreasuriesViewModel : ViewModelBase, IRecipient<AfterTreasuryInsertMessage>,
    ITreasuriesViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ITreasuryDataProvider _treasuryDataProvider;
    [ObservableProperty] private Treasury? _selectedTreasury;

    [ObservableProperty] private ObservableCollection<Treasury> _treasuries = [];

    public SavedTreasuriesViewModel(ITreasuryDataProvider treasuryDataProvider,
        INavigationService navigationService)
    {
        _treasuryDataProvider = treasuryDataProvider;
        _navigationService = navigationService;
        IsActive = true;
        Debug.WriteLine("SavedTreasuryVM constructed");
    }

    public async void Receive(AfterTreasuryInsertMessage message)
    {
        await HandleMessage(message);
    }

    [RelayCommand]
    public override async Task LoadAsync()
    {
        if (Treasuries.Any()) return;
        var treasuries = await _treasuryDataProvider.GetSavedAsync();
        foreach (var t in treasuries) Treasuries.Add(t);
    }

    private async Task HandleMessage(AfterTreasuryInsertMessage message)
    {
        Treasuries.Clear();
        await LoadAsync();
        var ust = message.Value;
        var found = Treasuries.FirstOrDefault(t => t.Cusip == ust.Cusip && t.IssueDate == ust.IssueDate);
        if (found is not null) SelectedTreasury = found;
        _navigationService.NavigateTo<SavedTreasuriesViewModel>();
    }
}
