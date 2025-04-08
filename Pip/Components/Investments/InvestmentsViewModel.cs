using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using Pip.DataAccess;
using Pip.Model;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments.Messages;
using Pip.UI.Components.Shared;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentsViewModel : PipViewModel, IPipRoute
{
    private readonly PipDbContext _dbContext;
    [GenerateProperty] private bool _isWaitIndicatorVisible;
    [GenerateProperty] private InvestmentItemViewModel? _selectedInvestment;

    public InvestmentsViewModel(DetailsViewModel detailsViewModel,
        PipDbContext dbContext)
    {
        _dbContext = dbContext;
        DetailsViewModel = detailsViewModel;
        Messenger.Default.Register<AfterInsertInvestmentMessage>(this, Receive);
    }

    public ObservableCollection<InvestmentItemViewModel> Investments { get; } = [];

    public DetailsViewModel DetailsViewModel { get; }

    public string View => nameof(InvestmentsView);

    public string Title => "Investments";

    public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Opportunity.svg");

    public override void Load()
    {
        if (Investments.Any()) return;

        foreach (Investment investment in _dbContext.Investments.AsEnumerable())
            Investments.Add(new InvestmentItemViewModel(investment));
    }

    public override async Task LoadAsync()
    {
        if (Investments.Any()) return;

        IEnumerable<Investment> inv = await Task.Run(() => _dbContext.Investments.AsEnumerable()).ConfigureAwait(false);

        Dispatcher.InvokeAsync(() =>
        {
            foreach (Investment investment in inv)
                Investments.Add(new InvestmentItemViewModel(investment));
        });
    }

    private void Receive(AfterInsertInvestmentMessage message)
    {
        Dispatcher.InvokeAsync(async () =>
        {
            await Task.Delay(1000);
            Investments.Clear();
            await LoadAsync();
            SelectedInvestment = Investments.FirstOrDefault(i => i.Id == message.Value.Id);
        });
    }

    [GenerateCommand]
    private void DataSourceRefresh(DataSourceRefreshArgs args)
    {
        Investments.Clear();
        Load();
    }

    [GenerateCommand]
    private void ValidateRow(RowValidationArgs args)
    {
        InvestmentItemViewModel? investmentItem = (InvestmentItemViewModel)args.Item;
        if (investmentItem.HasErrors) return;

        Investment inv = investmentItem.SyncToInvestment();
        if (args.IsNewItem)
            //_treasuryDataProvider.Add(inv);
            _dbContext.Investments.Add(inv);

        //_treasuryDataProvider.Save();
        _dbContext.SaveChanges();
    }

    [GenerateCommand]
    private void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        try
        {
            InvestmentItemViewModel? investmentItem = (InvestmentItemViewModel)args.Items.Single();
            //_treasuryDataProvider.Delete(investmentItem.SyncToInvestment());
            _dbContext.Investments.Remove(investmentItem.SyncToInvestment());
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
        }
    }
}