using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Pip.DataAccess;
using Pip.Model;

namespace Pip.WinUI.ViewModels;

public partial class MainViewModel(PipDbContext dbContext) : ObservableRecipient
{
	public ObservableCollection<Investment>? Investments =>
		field ??= new ObservableCollection<Investment>(dbContext.Investments.ToList());

	public static TreasuryType[] TreasuryTypes => Enum.GetValues<TreasuryType>();
}