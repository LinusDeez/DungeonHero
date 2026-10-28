using RogueSharp.Random;
using Roguelike.Core;
using RogueSharp;

namespace Roguelike.Core.Tests;

public class MapGeneratorTests
{
    private static DungeonMap CreateMap(int seed)
    {
        var generator = new MapGenerator(80, 48, 20, 13, 7, new DotNetRandom(seed));
        return generator.CreateMap();
    }

    [Fact]
    public void CreateMap_HasCorrectSize()
    {
        var map = CreateMap(seed: 42);

        Assert.Equal(80, map.Width);
        Assert.Equal(48, map.Height);
    }

    [Fact]
    public void CreateMap_HasAtLeastOneRoom()
    {
        var map = CreateMap(seed: 42);

        Assert.NotEmpty(map.Rooms);
    }

    [Fact]
    public void CreateMap_SameSeed_GivesSameMap()
    {
        var first = CreateMap(seed: 42);
        var second = CreateMap(seed: 42);

        Assert.Equal(first.ToString(), second.ToString());
    }

    // TODO: CreateMap_RoomCenterIsWalkable
    [Fact]
    public void CreateMap_RoomCenterIsWalkable()
    {
        var map = CreateMap(seed: 42);
        var room = map.Rooms[0];

        Assert.True(map.IsWalkable(room.Center.X, room.Center.Y));
    }

    [Fact]
    public void PlacePlayer_PutsPlayerInFirstRoomCenter()
    {
        var map = CreateMap(seed: 42);
        var player = new Player();

        map.PlacePlayer(player);

        Assert.Equal(map.Rooms[0].Center.X, player.X);
        Assert.Equal(map.Rooms[0].Center.Y, player.Y);
    }

    [Fact]
    public void CreateMap_AllRoomsAreReachable()
    {
        var map = CreateMap(seed: 42);
        var pathFinder = new PathFinder(map);
        var start = map.GetCell(map.Rooms[0].Center.X, map.Rooms[0].Center.Y);

        Assert.All(map.Rooms, room =>
        {
            var goal = map.GetCell(room.Center.X, room.Center.Y);
            Assert.NotNull(pathFinder.TryFindShortestPath(start, goal));
        });
    }

}
