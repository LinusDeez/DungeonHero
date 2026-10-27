namespace Roguelike.Core;

public class CommandSystem(GameState state)
{
    public bool MovePlayer(Direction direction)
    {
        int x = state.Player.X;
        int y = state.Player.Y;

        switch (direction)
        {
            case Direction.Up:
                y = y - 1;
                break;
            case Direction.Down:
                y = y + 1;
                break;
            case Direction.Left:
                x = x - 1;
                break;
            case Direction.Right:
                x = x + 1;
                break;
        }

        return state.Map.SetActorPosition(state.Player, x, y);
    }
}
