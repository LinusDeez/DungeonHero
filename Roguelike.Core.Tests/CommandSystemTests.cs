using Roguelike.Core;

namespace Roguelike.Core.Tests;

public class CommandSystemTests
{
    [Fact]
    public void MovePlayer_Right_IncreasesX()
    {
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        var map = TestMaps.CreateOpenMap(80, 48);
        map.SetActorPosition(player, player.X, player.Y);
        var commands = new CommandSystem(new GameState(map, player));

        bool moved = commands.MovePlayer(Direction.Right);

        Assert.True(moved);
        Assert.Equal(11, player.X);
    }

    [Fact]
    public void MovePlayer_IntoWall_DoesNotMove()
    {
        var player = new Player();
        player.X = 1;
        var map = TestMaps.CreateOpenMap(80, 48);
        map.SetActorPosition(player, player.X, player.Y);
        var commands = new CommandSystem(new GameState(map, player));

        bool moved = commands.MovePlayer(Direction.Left);
        
        Assert.False(moved);
        Assert.Equal(1, player.X);
    }
}
