using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Home;

[GenerateViewModel]
public partial class HomeViewModel : PipViewModel, IPipPage
{
	public string View => "HomeView";
	public string Title => "Home";
	public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/Icon Builder/Actions_Home.svg");
}