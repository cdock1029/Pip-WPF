using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Pip.Model;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Investments;

public class InvestmentItemViewModel : PipViewModel, INotifyDataErrorInfo
{
	private readonly Dictionary<string, List<string>> _errors = new();

	[Display] public required Investment Investment { get; init; }


	[Display(GroupName = "[Investment parameters]")]
	public string? Confirmation
	{
		get => Investment.Confirmation;
		set
		{
			Investment.Confirmation = value;
			RaisePropertyChanged();
			ClearErrors();
			if (string.IsNullOrEmpty(value) || value.Length != 5) AddError("Confirmation must be of length 5");
			OnErrorsChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]")]
	public int Reinvestments
	{
		get => Investment.Reinvestments;
		set
		{
			Investment.Reinvestments = value;
			RaisePropertyChanged();
			ClearErrors();
			if (value > 5) AddError("Max re-investments is 5");
			OnErrorsChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]")]
	[DataType(DataType.Currency)]
	public int Par
	{
		get => Investment.Par;
		set
		{
			Investment.Par = value;
			RaisePropertyChanged();
		}
	}

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public string Cusip => Investment.Cusip;

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public DateOnly IssueDate => Investment.IssueDate;

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	[UsedImplicitly]
	public DateOnly? MaturityDate => Investment.MaturityDate;

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public string? Term => Investment.SecurityTerm;

	[Display(GroupName = "[Investment parameters]/[Treasury]")]
	public TreasuryType Type => Investment.Type;


	public int TermSpan => MaturityDate is null ? 0 : MaturityDate.Value.DayNumber - IssueDate.DayNumber;

	public IEnumerable GetErrors(string? propertyName)
	{
		return string.IsNullOrEmpty(propertyName)
			? _errors.SelectMany(p => p.Value)
			: _errors.GetValueOrDefault(propertyName!, []);
	}

	public bool HasErrors => _errors.Any();

	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	private void AddError(string errorMsg, [CallerMemberName] string propertyName = "")
	{
		if (!_errors.ContainsKey(propertyName)) _errors.Add(propertyName, []);
		_errors[propertyName].Add(errorMsg);
		OnErrorsChanged(propertyName);
	}

	private void ClearErrors([CallerMemberName] string propertyName = "")
	{
		_errors.Remove(propertyName);
	}

	private void OnErrorsChanged([CallerMemberName] string propertyName = "")
	{
		ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
	}
}