using System.Runtime.InteropServices;

namespace Td;

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

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Rect(int left, int top, int right, int bottom)
{
	public int Left = left;
	public int Top = top;
	public int Right = right;
	public int Bottom = bottom;
}

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Point(int x, int y)
{
	public int X = x;
	public int Y = y;
}