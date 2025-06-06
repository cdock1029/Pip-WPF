using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using Microsoft.EntityFrameworkCore;
using Pip.UI.Components.Details;
using Pip.UI.Components.Investments.Messages;
using Pip.UI.Components.Shared;
using Investment = Pip.UI.Models.Investment;
using PipDbContext = Pip.UI.Data.PipDbContext;

namespace Pip.UI.Components.Investments;

[GenerateViewModel]
public partial class InvestmentsViewModel : PipViewModel, IPipRoute
{
    private readonly PipDbContext _dbContext;
    [GenerateProperty] private ObservableCollection<Investment>? _investments;
    [GenerateProperty] private Investment? _selectedInvestment;

    public InvestmentsViewModel(DetailsViewModel detailsViewModel,
        PipDbContext dbContext)
    {
        _dbContext = dbContext;
        DetailsViewModel = detailsViewModel;
        _dbContext.Investments.Load();
        Investments = _dbContext.Investments.Local.ToObservableCollection();
        Messenger.Default.Register<AfterInsertInvestmentMessage>(this, Receive);
    }

    public DetailsViewModel DetailsViewModel { get; }

    public string View => nameof(InvestmentsView);

    public string Title => "Investments";

    public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/Business Objects/BO_Opportunity.svg");

    private void Receive(AfterInsertInvestmentMessage message)
    {
        SelectedInvestment = Investments?.FirstOrDefault(i => i.Id == message.Value.Id);
    }

    [GenerateCommand]
    private void DataSourceRefresh(DataSourceRefreshArgs args)
    {
    }

    [GenerateCommand]
    private void ValidateRow(RowValidationArgs args)
    {
        Investment investmentItem = (Investment)args.Item;
        //if (investmentItem.HasErrors) return;

        if (args.IsNewItem)
            _dbContext.Investments.Add(investmentItem);

        _dbContext.SaveChanges();
    }

    [GenerateCommand]
    private void ValidateRowDeletion(ValidateRowDeletionArgs args)
    {
        try
        {
            Investment investmentItem = (Investment)args.Items.Single();
            _dbContext.Investments.Local.Remove(investmentItem);
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            args.ResultAsync = Task.FromResult(new ValidationErrorInfo($"Error Deleting:\n{e.Message}"));
        }
    }
}