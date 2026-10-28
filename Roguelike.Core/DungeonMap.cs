using RogueSharp;
using RogueSharp.Random;

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

    public void PlacePlayer(Player player)
    {
        player.X = Rooms[0].Center.X;
        player.Y = Rooms[0].Center.Y;
        SetActorPosition(player, player.X, player.Y);

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

    public List<Rectangle> Rooms { get; } = [];

    private readonly List<Monster> _monsters = [];
    public IReadOnlyList<Monster> Monsters => _monsters;

    public void AddMonster(Monster monster)
    {
        _monsters.Add(monster);
        SetIsWalkable(monster.X, monster.Y, false);
    }

    public Point? GetRandomWalkableLocationInRoom(Rectangle room, IRandom random)
    {
        for (int i = 0; i < 100; i++)
        {
            int x = random.Next(room.Left + 1, room.Right - 1);
            int y = random.Next(room.Top + 1, room.Bottom - 1);

            if (IsWalkable(x, y))
            {
                return new Point(x, y);
            }
            
        }

        return null;
    }

    public IEnumerable<Monster> GetMonstersInFov()
    {
        return _monsters.Where(monster => IsInFov(monster.X, monster.Y));
    }

    public Monster? GetMonsterAt(int x, int y)
    {
        return _monsters.FirstOrDefault(monster => monster.X == x && monster.Y == y);
    }

    public void RemoveMonster(Monster monster)
    {
        _monsters.Remove(monster);
        SetIsWalkable(monster.X, monster.Y, true);
    }
}
