using Roguelike.Core;

namespace Roguelike.Core.Tests;

public class PlayerTests
{
    [Fact]
    public void NewPlayer_HasFullHealth()
    {
        var player = new Player();

        Assert.Equal(player.MaxHealth, player.Health);
    }
}
