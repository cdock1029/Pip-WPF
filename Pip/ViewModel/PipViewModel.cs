using System.Windows.Threading;
using DevExpress.Mvvm;
using Application = System.Windows.Application;

namespace Pip.UI.ViewModel;

public abstract class PipViewModel : ViewModelBase
{
	protected readonly Dispatcher Dispatcher = Application.Current.Dispatcher;

	protected IDialogService DialogService => GetService<IDialogService>();

	public virtual Task LoadAsync()
	{
		return Task.CompletedTask;
	}
}