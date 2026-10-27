namespace Roguelike.Core;

public class GameState(DungeonMap map, Player player)
{
    public DungeonMap Map { get; } = map;
    public Player Player {get; } = player;
}