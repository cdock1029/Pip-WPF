namespace Pip.UI.Components.Shared;

public interface IPipPage
{
	public string? View { get; }
	public string Title { get; }
	public Uri? Image { get; }
}