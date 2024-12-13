using System.Windows.Threading;
using DevExpress.Mvvm;
using Application = System.Windows.Application;

namespace Pip.UI.ViewModel;

public abstract class PipViewModel : BindableBase
{
	protected readonly Dispatcher Dispatcher = Application.Current.Dispatcher;

	public virtual Task LoadAsync()
	{
		return Task.CompletedTask;
	}
}