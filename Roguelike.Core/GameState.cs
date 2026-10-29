namespace Roguelike.Core;

public class GameState
{
    public DungeonMap Map { get; private set; }
    public Player Player {get; }
    public MessageLog Messages { get; } = new();
    public SchedulingSystem Schedule { get; } = new();
    public bool GameIsOver => Player.Health <= 0;
    public int Level { get; private set; } = 1;

    public GameState(DungeonMap map, Player player)
    {
        Map = map;
        Player = player;

        Schedule.Add(player);
        ScheduleMonster();
    }

    public void ChangeLevel(DungeonMap newMap)
    {
        Map = newMap;
        Level++;
        Schedule.Clear();
        Map.PlacePlayer(Player);
        ScheduleMonster();
        Messages.Add($"You descend to level {Level}.");
    }

    private void ScheduleMonster()
    {
        foreach (Monster monster in Map.Monsters)
        {
            Schedule.Add(monster);
        }
    }
}
