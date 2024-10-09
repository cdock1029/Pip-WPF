using DevExpress.Mvvm.CodeGenerators;
using Pip.Model;

namespace Pip.UI.Components.Details;

[GenerateViewModel]
public partial class DetailsViewModel
{
	[GenerateProperty] private Treasury? _treasuryDetailsSelected;
}