namespace DungeonHero.Core.Tests;

public class DoorTests
{
    [Fact]
    public void SetActorPosition_OntoClosedDoor_OpensIt()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var door = new Door(11, 10);
        map.AddDoor(door);

        var player = new Player();
        player.X = 10;
        player.Y = 10;
        map.SetActorPosition(player, player.X, player.Y);

        map.SetActorPosition(player, 11, 10);

        Assert.True(door.IsOpen);
        Assert.True(map.IsTransparent(11, 10));
    }

    [Fact]
    public void AddDoor_MakesCellOpaque()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var door = new Door(11, 10);
        map.AddDoor(door);

        Assert.False(map.IsTransparent(11, 10));
    }
}
