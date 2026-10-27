using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using Roguelike.Core;

namespace Roguelike.Client;

public class RootScreen : ScreenObject
{
    private readonly GameState _state;
    private readonly CommandSystem _commands;
    private readonly SadConsole.Console _mapConsole;

    public RootScreen(GameState state, CommandSystem commands)
    {
        _state = state;
        _commands = commands;

        var inventoryConsole = new SadConsole.Console(80, 11);
        inventoryConsole.Position = new Point(0, 0);
        inventoryConsole.Fill(background: Swatch.DbWood);
        inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);
        Children.Add(inventoryConsole);

        _mapConsole = new SadConsole.Console(state.Map.Width, state.Map.Height);
        _mapConsole.Position = new Point(0, 11);
        MapRenderer.Draw(state, _mapConsole);
        Children.Add(_mapConsole);

        var messageConsole = new SadConsole.Console(80, 11);
        messageConsole.Position = new Point(0, _mapConsole.Position.Y + _mapConsole.Height);
        messageConsole.Fill(background: Swatch.DbDeepWater);
        messageConsole.Print(1, 1, "Messages", Colors.TextHeading);
        Children.Add(messageConsole);

        var statsConsole = new SadConsole.Console(20, 70);
        statsConsole.Position = new Point(80, 0);
        statsConsole.Fill(background: Swatch.DbOldStone);
        statsConsole.Print(1, 1, "Stats", Colors.TextHeading);
        Children.Add(statsConsole);
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool moved = false;

        if (keyboard.IsKeyPressed(Keys.Up))
        {
            moved = _commands.MovePlayer(Core.Direction.Up);
        }
        else if (keyboard.IsKeyPressed(Keys.Down))
        {
            moved = _commands.MovePlayer(Core.Direction.Down);
        }
        else if (keyboard.IsKeyPressed(Keys.Left))
        {
            moved = _commands.MovePlayer(Core.Direction.Left);
        }
        else if (keyboard.IsKeyPressed(Keys.Right))
        {
            moved = _commands.MovePlayer(Core.Direction.Right);
        }
        else if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }

        if (moved)
        {
            MapRenderer.Draw(_state, _mapConsole);
        }

        return true;
    }
}
