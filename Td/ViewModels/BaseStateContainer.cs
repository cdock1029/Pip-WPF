namespace Td.ViewModels;

public abstract class BaseStateContainer
{
	protected void NotifyStateChanged()
	{
		OnChange?.Invoke();
	}

	public event Action? OnChange;

	public virtual void LoadData()
	{
	}
}