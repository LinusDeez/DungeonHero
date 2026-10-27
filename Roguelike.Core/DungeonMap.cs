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

    public void SetIsWalkable(int x, int y, bool isWalkable)
    {
        ICell cell = GetCell(x, y);
        SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
    }

    public bool SetActorPosition(Actor actor, int x, int y)
    {
        if (!IsWalkable(x, y))
        {
            return false;
        }

        SetIsWalkable(actor.X, actor.Y, true);
        actor.X = x;
        actor.Y = y;
        SetIsWalkable(actor.X, actor.Y, false);

        if (actor is Player player)
        {
            UpdatePlayerFov(player);
        }

        return true;
    }
}