using DungeonHero.Core;

namespace DungeonHero.Core.Tests;

public class PlayerTests
{
    [Fact]
    public void NewPlayer_HasFullHealth()
    {
        var player = new Player();

        Assert.Equal(player.MaxHealth, player.Health);
    }
}
