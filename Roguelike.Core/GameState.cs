namespace Roguelike.Core;

public class GameState
{
    public DungeonMap Map { get; }
    public Player Player {get; }
    public MessageLog Messages { get; } = new();
    public SchedulingSystem Schedule { get; } = new();

    public GameState(DungeonMap map, Player player)
    {
        Map = map;
        Player = player;

        Schedule.Add(player);
        foreach (Monster monster in map.Monsters)
        {
            Schedule.Add(monster);
        }
    }
}
