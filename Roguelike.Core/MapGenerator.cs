using RogueSharp;
using RogueSharp.Random;

namespace Roguelike.Core;

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

        return map;
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
}
