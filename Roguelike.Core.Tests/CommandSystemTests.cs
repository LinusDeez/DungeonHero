using Roguelike.Core;
using RogueSharp.Random;
using Roguelike.Core.Monsters;

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
        var commands = new CommandSystem(new GameState(map, player), new DotNetRandom(42));

        bool moved = commands.MovePlayer(Direction.Right);

        Assert.True(moved);
        Assert.Equal(11, player.X);
    }

    [Fact]
    public void MovePlayer_IntoWall_DoesNotMove()
    {
        var player = new Player();
        player.X = 1;
        player.Y = 10;
        var map = TestMaps.CreateOpenMap(80, 48);
        map.SetActorPosition(player, player.X, player.Y);
        var commands = new CommandSystem(new GameState(map, player), new DotNetRandom(42));

        bool moved = commands.MovePlayer(Direction.Left);
        
        Assert.False(moved);
        Assert.Equal(1, player.X);
    }

    [Fact]
    public void MovePlayer_IntoMonster_AttacksInsteadOfMoving()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        player.Attack = 1;
        player.AttackChance = 100;
        map.SetActorPosition(player, player.X, player.Y);

        var goblin = Goblin.Create(1, new DotNetRandom(42));
        goblin.X = 11;
        goblin.Y = 10;
        goblin.Defense = 0;
        map.AddMonster(goblin);

        var commands = new CommandSystem(new GameState(map, player), new DotNetRandom(42));
        int healthBefore = goblin.Health;

        commands.MovePlayer(Direction.Right);

        // Player didn't move.
        Assert.Equal(10, player.X);
        // Exactly 1 damage dealt.
        Assert.Equal(healthBefore - 1, goblin.Health);
    }

    [Fact]
    public void Attack_KillsMonster_RemovesItFromMap()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        player.Attack = 20;
        player.AttackChance = 100;
        map.SetActorPosition(player, player.X, player.Y);

        var goblin = Goblin.Create(1, new DotNetRandom(42));
        goblin.X = 11;
        goblin.Y = 10;
        goblin.Defense = 0;
        map.AddMonster(goblin);

        var commands = new CommandSystem(new GameState(map, player), new DotNetRandom(42));
        int goldCombinedBefore = goblin.Gold + player.Gold;

        commands.MovePlayer(Direction.Right);

        Assert.Empty(map.Monsters);
        Assert.Equal(goldCombinedBefore, player.Gold);
    }

    [Fact]
    public void EndPlayerTurn_RunsMonstersUntilPlayerIsNext()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        map.SetActorPosition(player, player.X, player.Y);

        var goblin = Goblin.Create(1, new DotNetRandom(42));
        goblin.X = 30;
        goblin.Y = 30;
        map.AddMonster(goblin);

        var state = new GameState(map, player);
        var commands = new CommandSystem(state, new DotNetRandom(42));
        commands.ActivateMonsters();

        commands.EndPlayerTurn();

        Assert.Equal(20, state.Schedule.Time);
}
}
