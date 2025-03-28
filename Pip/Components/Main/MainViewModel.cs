using System.Collections.ObjectModel;
using System.Diagnostics;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core.Native;
using Pip.UI.Components.Auctions;
using Pip.UI.Components.Details;
using Pip.UI.Components.Home;
using Pip.UI.Components.Investments;
using Pip.UI.Components.Investments.Messages;
using Pip.UI.Components.Search;
using Pip.UI.Components.Shared;

namespace Pip.UI.Components.Main;

[GenerateViewModel]
public partial class MainViewModel : PipViewModel
{
    [GenerateProperty] private IPipRoute _selectedRoute;

    public MainViewModel(InvestmentsViewModel investmentsViewModel,
        SearchViewModel searchViewModel,
        AuctionsViewModel auctionsViewModel,
        DetailsViewModel detailsViewModel,
        HomeViewModel homeViewModel)
    {
        SearchViewModel = searchViewModel;
        DetailsViewModel = detailsViewModel;
        InvestmentsViewModel = investmentsViewModel;
        HomeViewModel = homeViewModel;
        AuctionsViewModel = auctionsViewModel;

        SelectedRoute = HomeViewModel;

        Messenger.Default.Register<AfterInsertInvestmentMessage>(this, ReceiveAfterInvestmentMessage);
    }

    public ObservableCollection<string> SearchResults { get; } =
        ["Issue Date 1", "Issue Date 2", "Issue Date 3", "Issue Date 4", "Issue Date 5"];

    public HomeViewModel HomeViewModel { get; }
    public InvestmentsViewModel InvestmentsViewModel { get; }
    public SearchViewModel SearchViewModel { get; }
    public DetailsViewModel DetailsViewModel { get; }
    public AuctionsViewModel AuctionsViewModel { get; }

    private INavigationService NavigationService => GetService<INavigationService>();

    public IPipRoute[] Routes => [HomeViewModel, AuctionsViewModel, InvestmentsViewModel];

    [GenerateCommand]
    private void ShowForm()
    {
        InvestmentItemViewModel model = new()
        {
            Cusip = "",
            IssueDate = DateTime.Now.ToDateOnly()
        };

        MessageResult result =
            DialogService.ShowDialog(MessageButton.OKCancel, "Investment form", nameof(InvestmentForm), model);

        Debug.WriteLine(
            $"result: {result}, model par: {model.Par}, confirmation: {model.Confirmation}, re-investments: {model.Reinvestments}");
    }

    [GenerateCommand]
    private void NavigateToSelected()
    {
        NavigationService.Navigate(SelectedRoute.View, SelectedRoute);
    }

    private void ReceiveAfterInvestmentMessage(AfterInsertInvestmentMessage msg)
    {
        SelectedRoute = InvestmentsViewModel;
        //NavigateInvestments();
    }
}