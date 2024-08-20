using System.Runtime.InteropServices;

namespace Pip.UI.View.Types;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct WindowPlacement
{
    public int length;
    public int flags;
    public int showCmd;
    public Point minPosition;
    public Point maxPosition;
    public Rect normalPosition;
}
