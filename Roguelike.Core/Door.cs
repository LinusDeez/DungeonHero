namespace Roguelike.Core;

public class Door(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public bool IsOpen { get; set; }
}