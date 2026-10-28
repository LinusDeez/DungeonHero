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
    private readonly SadConsole.Console _statsConsole;
    private readonly SadConsole.Console _messageConsole;

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

        _messageConsole = new SadConsole.Console(80, 11);
        _messageConsole.Position = new Point(0, _mapConsole.Position.Y + _mapConsole.Height);
        MessageRenderer.Draw(state.Messages, _messageConsole);
        Children.Add(_messageConsole);

        _statsConsole = new SadConsole.Console(20, 70);
        _statsConsole.Position = new Point(80, 0);
        StatsRenderer.Draw(state, _statsConsole);
        Children.Add(_statsConsole);
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
            _commands.EndPlayerTurn();
            MapRenderer.Draw(_state, _mapConsole);
            StatsRenderer.Draw(_state, _statsConsole);
            MessageRenderer.Draw(_state.Messages, _messageConsole);
        }

        return true;
    }
}
