using System.Windows.Media.Animation;
using DevExpress.Xpf.WindowsUI;

namespace Pip.UI.Resources;

public class FrameAnimationSelector : AnimationSelector
{
    public Storyboard ForwardStoryboard { get; set; } = null!;

    public Storyboard BackStoryboard { get; set; } = null!;

    protected override Storyboard SelectStoryboard(FrameAnimation animation)
    {
        return animation.Direction == AnimationDirection.Forward
            ? ForwardStoryboard
            : BackStoryboard;
    }
}