using System.Runtime.InteropServices;

namespace Pip.UI.View.Types;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Rect(int left, int top, int right, int bottom)
{
    public int Left = left;
    public int Top = top;
    public int Right = right;
    public int Bottom = bottom;
}
