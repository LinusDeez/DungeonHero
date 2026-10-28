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

        return map;
    }

    private static void CreateRoom(DungeonMap map, Rectangle room)
    {
        for (int x = room.Left + 1; x < room.Right; x++)
        {
            for (int y = room.Top +1; y < room.Bottom; y++)
            {
                map.SetCellProperties(x, y, true, true, true);
            }
        }
    }
}
