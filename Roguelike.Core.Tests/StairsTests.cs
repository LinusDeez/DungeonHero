namespace Roguelike.Core.Tests;

public class StairsTests
{
    [Fact]
    public void Stairs_WithSameValues_AreEqual()
    {
        var a = new Stairs(5, 7, IsUp: false);
        var b = new Stairs(5, 7, IsUp: false);

        // Record: Werte zählen, nicht das Objekt
        Assert.Equal(a, b);
    }

    [Fact]
    public void CanMoveDownToNextLevel_PlayerOnStairs_ReturnsTrue()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        map.StairsDown = new Stairs(12, 10, IsUp: false);
        var player = new Player();
        player.X = 12;
        player.Y = 10;

        Assert.True(map.CanMoveDownToNextLevel(player));
    }

    [Fact]
    public void CanMoveDownToNextLevel_PlayerElsewhere_ReturnsFalse()
    {
        // TODO: wie oben, aber der Spieler steht woanders
        var map = TestMaps.CreateOpenMap(80, 48);
        map.StairsDown = new Stairs(12, 10, IsUp: false);
        var player = new Player();
        player.X = 11;
        player.Y = 10;

        Assert.False(map.CanMoveDownToNextLevel(player));
    }
}
