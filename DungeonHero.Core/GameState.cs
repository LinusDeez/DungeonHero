namespace DungeonHero.Core;

public class GameState
{
    public DungeonMap Map { get; private set; }
    public int Level { get; private set; } = 1;
    public Player Player { get; }
    public MessageLog Messages { get; } = new();
    public SchedulingSystem Schedule { get; } = new();
    public bool GameIsOver => Player.Health <= 0;

    public GameState(DungeonMap map, Player player)
    {
        Map = map;
        Player = player;

        Schedule.Add(player);
        ScheduleMonsters();
    }

    public void ChangeLevel(DungeonMap newMap)
    {
        Map = newMap;
        Level++;
        Schedule.Clear();
        Map.PlacePlayer(Player);
        ScheduleMonsters();
        Messages.Add($"You descend to level {Level}.");
    }

    private void ScheduleMonsters()
    {
        foreach (Monster monster in Map.Monsters)
        {
            Schedule.Add(monster);
        }
    }
}
