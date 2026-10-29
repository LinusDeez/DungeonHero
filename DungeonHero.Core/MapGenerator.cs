using RogueSharp;
using RogueSharp.Random;
using RogueSharp.DiceNotation;
using DungeonHero.Core.Monsters;

namespace DungeonHero.Core;

public class MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize, IRandom random)
{
    public DungeonMap CreateMap()
    {
        var map = new DungeonMap();
        map.Initialize(width, height);

        for (int i = 0; i < maxRooms; i++)
        {
            int roomWidth = random.Next(roomMinSize, roomMaxSize);
            int roomHeight = random.Next(roomMinSize, roomMaxSize);
            int roomX = random.Next(0, width - roomWidth - 1);
            int roomY = random.Next(0, height - roomHeight - 1);

            var newRoom = new Rectangle(roomX, roomY, roomWidth, roomHeight);

            bool overlaps = map.Rooms.Any(room => newRoom.Intersects(room));
            if (!overlaps)
            {
                map.Rooms.Add(newRoom);
            }
        }

        foreach (Rectangle room in map.Rooms)
        {
            CreateRoom(map, room);
        }

        for (int r = 1; r < map.Rooms.Count; r++)
        {
            var previous = map.Rooms[r-1].Center;
            var current = map.Rooms[r].Center;

            if (random.Next(1, 2) == 1)
            {
                CreateHorizontalTunnel(map, previous.X, current.X, previous.Y);
                CreateVerticalTunnel(map, previous.Y, current.Y, current.X);
            }
            else
            {
                CreateVerticalTunnel(map, previous.Y, current.Y, previous.X);
                CreateHorizontalTunnel(map, previous.X, current.X, current.Y);
            }
        }

        foreach (Rectangle room in map.Rooms)
        {
            CreateDoors(map, room);
        }

        CreateStairs(map);

        return map;
    }

    public void PlaceMonsters(DungeonMap map, int level)
    {
        // Starting room stays empty.
        for (int r = 1; r < map.Rooms.Count; r++)
        {
            Rectangle room = map.Rooms[r];

            if (Dice.Roll("1D10", random) < 7)
            {
                int count = Dice.Roll("1D4", random);

                for (int i = 0; i < count; i++)
                {
                    Point? location = map.GetRandomWalkableLocationInRoom(room, random);

                if (location.HasValue)
                    {
                        var goblin = Goblin.Create(level, random);
                        goblin.X = location.Value.X;
                        goblin.Y = location.Value.Y;
                        map.AddMonster(goblin);
                    }
                }
            }
        }
    }


    private static void CreateRoom(DungeonMap map, Rectangle room)
    {
        for (int x = room.Left + 1; x < room.Right; x++)
        {
            for (int y = room.Top +1; y < room.Bottom; y++)
            {
                map.SetCellProperties(x, y, true, true, false);
            }
        }
    }

    private static void CreateHorizontalTunnel(DungeonMap map, int xStart, int xEnd, int y)
    {
        for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
        {
            map.SetCellProperties(x, y, true, true, false);
        }
    }

    private static void CreateVerticalTunnel(DungeonMap map, int yStart, int yEnd, int x)
    {
        for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
        {
            map.SetCellProperties(x, y, true, true, false);
        }
    }

    private static void CreateDoors(DungeonMap map, Rectangle room)
    {
        for (int x = room.Left; x <= room.Right; x++)
        {
            TryPlaceDoor(map, x, room.Top);
            TryPlaceDoor(map, x, room.Bottom);
        }

        for (int y = room.Top; y <= room.Bottom; y++)
        {
            TryPlaceDoor(map, room.Left, y);
            TryPlaceDoor(map, room.Right, y);
        }
    }

    private static void TryPlaceDoor(DungeonMap map, int x, int y)
    {
        // Wall
        if (!map.IsWalkable(x, y))
        {
            return;
        }

        bool doorNearby = map.GetDoor(x, y) != null
            || map.GetDoor(x - 1, y) != null
            || map.GetDoor(x + 1, y) != null
            || map.GetDoor(x, y - 1) != null
            || map.GetDoor(x, y + 1) != null;
        if (doorNearby)
        {
            return;
        }

        bool left = map.IsWalkable(x - 1, y);
        bool right = map.IsWalkable(x + 1, y);
        bool top = map.IsWalkable(x, y - 1);
        bool bottom = map.IsWalkable(x, y + 1);

        bool isSideWallGap = left && right && !top && !bottom;
        bool isTopBottomWallGap = !left && !right && top && bottom;

        if (isSideWallGap || isTopBottomWallGap)
        {
            map.AddDoor(new Door(x, y));
        } 
    }

    private static void CreateStairs(DungeonMap map)
    {
        Point first = map.Rooms[0].Center;
        Point last = map.Rooms[^1].Center;

        map.StairsUp = new Stairs(first.X + 1, first.Y, IsUp: true);
        map.StairsDown = new Stairs(last.X, last.Y, IsUp: false);
    }
}
