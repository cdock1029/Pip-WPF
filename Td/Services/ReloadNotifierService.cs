namespace Td.Services;

public class ReloadNotifierService
{
	public bool IsReloading { get; private set; }

	public void Update(bool value)
	{
		IsReloading = value;
		Notify?.Invoke(this, value);
	}

	public event EventHandler<bool>? Notify;
}