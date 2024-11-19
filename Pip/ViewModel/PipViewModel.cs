using DevExpress.Mvvm;

namespace Pip.UI.ViewModel;

public abstract class PipViewModel : BindableBase
{
	public virtual Task LoadAsync()
	{
		return Task.CompletedTask;
	}
}
