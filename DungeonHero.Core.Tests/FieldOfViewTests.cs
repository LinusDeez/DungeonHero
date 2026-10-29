using DungeonHero.Core;

namespace DungeonHero.Core.Tests;

public class FieldOfViewTests
{
    [Fact]
    public void PlayerCellIsInFov()
    {
        var player = new Player();
        var map = TestMaps.CreateOpenMap(80, 48);

        map.UpdatePlayerFov(player);

        Assert.True(map.IsInFov(player.X, player.Y));
    }

    [Fact]
    public void FarAwayCellIsNotInFov()
    {
        var player = new Player();
        var map = TestMaps.CreateOpenMap(80, 48);

        map.UpdatePlayerFov(player);

        Assert.False(map.IsInFov(70,40));
    }
}