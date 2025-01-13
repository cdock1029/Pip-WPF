namespace Td.Components;

public abstract class BaseStateComponent<T> : ComponentBase, IDisposable where T : BaseStateContainer
{
	[Inject] public T StateContainer { get; set; } = null!;

	public void Dispose()
	{
		StateContainer.OnChange -= StateHasChanged;
	}


	protected override void OnInitialized()
	{
		StateContainer.OnChange += StateHasChanged;
		StateContainer.LoadData();
	}
}