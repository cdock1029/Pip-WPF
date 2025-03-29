using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Xpf.Core;
using Pip.UI.Components.Shared;

namespace Pip.UI.Components.Home;

[GenerateViewModel]
public partial class HomeViewModel : PipViewModel, IPipRoute
{
    public string View => nameof(HomeView);

    public string Title => "Home";

    public Uri Image { get; } = DXImageHelper.GetImageUri("SvgImages/RichEdit/InsertTableOfFigures.svg");
}