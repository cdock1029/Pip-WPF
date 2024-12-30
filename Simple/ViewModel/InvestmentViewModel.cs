using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Mvvm.Xpf;
using Microsoft.EntityFrameworkCore;
using Pip.DataAccess;
using Pip.Model;

namespace Simple.ViewModel;

[GenerateViewModel]
public partial class InvestmentViewModel
{
	private PipDbContext? _context;
	private IList<Investment>? _itemsSource;

	public IList<Investment>? ItemsSource
	{
		get
		{
			if (_itemsSource != null || ViewModelBase.IsInDesignMode) return _itemsSource;

			_context = new PipDbContext(new DbContextOptions<PipDbContext>());
			_itemsSource = _context.Investments.ToList();

			return _itemsSource;
		}
	}

	[GenerateCommand]
	private void ValidateRow(RowValidationArgs args)
	{
		var item = (Investment)args.Item;
		if (args.IsNewItem)
			_context?.Investments.Add(item);
		_context?.SaveChanges();
	}

	[GenerateCommand]
	private void ValidateRowDeletion(ValidateRowDeletionArgs args)
	{
		var item = (Investment)args.Items.Single();
		_context?.Investments.Remove(item);
		_context?.SaveChanges();
	}

	[GenerateCommand]
	private void DataSourceRefresh(DataSourceRefreshArgs args)
	{
		_itemsSource = null;
		_context = null;
		RaisePropertyChanged(new PropertyChangedEventArgs(nameof(ItemsSource)));
	}
}