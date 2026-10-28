using RogueSharp.Random;
using Roguelike.Core;

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
}
