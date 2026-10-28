using RogueSharp;
using Roguelike.Core;

namespace Roguelike.Core.Tests;

public static class TestMaps
{
    public static DungeonMap CreateOpenMap(int width, int height)
    {
        var map = new DungeonMap();
        map.Initialize(width, height);

        foreach (ICell cell in map.GetAllCells())
        {
            bool isBorder = cell.X == 0 || cell.Y == 0 || cell.X == width - 1 || cell.Y == height - 1;
            map.SetCellProperties(cell.X, cell.Y, !isBorder, !isBorder, true);
        }

        return map;
    }
}
