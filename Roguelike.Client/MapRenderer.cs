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

        foreach (Door door in state.Map.Doors)
        {
            DrawDoor(door, state.Map, console);
        }

        foreach (Monster monster in state.Map.Monsters)
        {
            DrawMonster(monster, state.Map, console);
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

    private static void DrawMonster(Monster monster, DungeonMap map, SadConsole.Console console)
    {
        if (map.IsInFov(monster.X, monster.Y))
        {
            console.SetGlyph(monster.X, monster.Y, MonsterLook.Glyph(monster), MonsterLook.Color(monster), Colors.FloorBackgroundFov);
        }
        else if (map.GetCell(monster.X, monster.Y).IsExplored)
        {
            console.SetGlyph(monster.X, monster.Y, '.', Colors.Floor, Colors.FloorBackground);
        }
    }

    private static void DrawDoor(Door door, DungeonMap map, SadConsole.Console console)
    {
        ICell cell = map.GetCell(door.X, door.Y);
        if (!cell.IsExplored)
        {
            return;
        }

        char glyph = door.IsOpen ? '-' : '+';

        if (cell.IsInFov)
        {
            console.SetGlyph(door.X, door.Y, glyph, Colors.DoorFov, Colors.DoorBackgroundFov);
        }
        else
        {
            console.SetGlyph(door.X, door.Y, glyph, Colors.Door, Colors.DoorBackground);
        }
    }
}
