using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using DevExpress.Mvvm;
using Application = System.Windows.Application;

namespace Pip.UI.ViewModel;

public abstract class PipViewModel : ViewModelBase, INotifyDataErrorInfo
{
	private readonly Dictionary<string, List<string>> _errors = new();
	protected readonly Dispatcher Dispatcher = Application.Current.Dispatcher;

	protected IDialogService DialogService => GetService<IDialogService>();

	public bool HasErrors => _errors.Any();

	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	public IEnumerable GetErrors(string? propertyName)
	{
		return string.IsNullOrEmpty(propertyName)
			? _errors.SelectMany(p => p.Value)
			: _errors.GetValueOrDefault(propertyName, []);
	}

	protected void AddError(string errorMsg, [CallerMemberName] string propertyName = "")
	{
		if (!_errors.ContainsKey(propertyName)) _errors.Add(propertyName, []);
		_errors[propertyName].Add(errorMsg);
		OnErrorsChanged(propertyName);
	}

	protected void ClearErrors([CallerMemberName] string propertyName = "")
	{
		_errors.Remove(propertyName);
	}

	protected void OnErrorsChanged([CallerMemberName] string propertyName = "")
	{
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
	}

	public virtual Task LoadAsync()
	{
		return Task.CompletedTask;
	}

	public virtual void Load()
	{
	}
}