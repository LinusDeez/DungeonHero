using RogueSharp;

namespace Roguelike.Core;

public class DungeonMap : Map
{
    public void UpdatePlayerFov(Player player)
    {
        ComputeFov(player.X, player.Y, player.Awareness, true);

        foreach (ICell cell in GetAllCells())
        {
            if (IsInFov(cell.X, cell.Y))
            {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, isExplored: true);
            }
        }
    }
}