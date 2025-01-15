using DevExpress.Mvvm.CodeGenerators;
using Pip.UI.ViewModel;

namespace Pip.UI.Components.Home;

[GenerateViewModel]
public partial class HomeViewModel : PipViewModel
{
	public string Title => "Home page";
}