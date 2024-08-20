using System.Runtime.InteropServices;

namespace Pip.UI.View.Types;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Point(int x, int y)
{
    public int X = x;
    public int Y = y;
}
