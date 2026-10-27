using RogueSharp;
using Roguelike.Core;
using SadConsole;

namespace Roguelike.Client;

public static class MapRenderer
{
    public static void Draw(GameState state, SadConsole.Console console)
    {
        console.Clear();

        foreach (ICell cell in state.Map.GetAllCells())
        {
            DrawCell(cell, console);
        }

        console.SetGlyph(state.Player.X, state.Player.Y, '@', Colors.Player, Colors.FloorBackgroundFov);
    }

    private static void DrawCell(ICell cell, SadConsole.Console console)
    {
        if (!cell.IsExplored)
        {
            return;
        }

        if (cell.IsInFov)
        {
            if (cell.IsWalkable)
            {
                console.SetGlyph(cell.X, cell.Y, '.', Colors.FloorFov, Colors.FloorBackgroundFov);
            }
            else
            {
                console.SetGlyph(cell.X, cell.Y, '#', Colors.WallFov, Colors.WallBackgroundFov);
            }
        }
        else
        {
            if (cell.IsWalkable)
            {
                console.SetGlyph(cell.X, cell.Y, '.', Colors.Floor, Colors.FloorBackground);
            }
            else
            {
                console.SetGlyph(cell.X, cell.Y, '#', Colors.Wall, Colors.WallBackground);
            }
        }
    }
}
