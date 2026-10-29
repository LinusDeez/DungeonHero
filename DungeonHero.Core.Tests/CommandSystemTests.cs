using DungeonHero.Core;
using RogueSharp.Random;
using DungeonHero.Core.Monsters;

namespace DungeonHero.Core.Tests;

public class CommandSystemTests
{
    private static CommandSystem CreateCommands(GameState state)
    {
        var random = new DotNetRandom(42);
        var generator = new MapGenerator(80, 48, 20, 13, 7, random);
        return new CommandSystem(state, generator, random);
    }

    [Fact]
    public void MovePlayer_Right_IncreasesX()
    {
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        var map = TestMaps.CreateOpenMap(80, 48);
        map.SetActorPosition(player, player.X, player.Y);
        var commands = CreateCommands(new GameState(map, player));

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
        var commands = CreateCommands(new GameState(map, player));

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

        var commands = CreateCommands(new GameState(map, player));
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

        var commands = CreateCommands(new GameState(map, player));
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
        var commands = CreateCommands(state);
        commands.ActivateMonsters();

        commands.EndPlayerTurn();

        Assert.Equal(20, state.Schedule.Time);
    }

    [Fact]
    public void EndPlayerTurn_AdjacentGoblin_AttacksPlayer()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        player.Defense = 0;
        map.SetActorPosition(player, player.X, player.Y);

        var goblin = Goblin.Create(1, new DotNetRandom(42));
        goblin.X = 11;
        goblin.Y = 10;
        goblin.Attack = 1;
        goblin.AttackChance = 100;
        map.AddMonster(goblin);

        var state = new GameState(map, player);
        var commands = CreateCommands(state);
        commands.ActivateMonsters();

        commands.EndPlayerTurn();

        Assert.Equal(99, player.Health);
    }

    [Fact]
    public void EndPlayerTurn_GoblinNearby_MovesTowardsPlayer()
    {
        var map = TestMaps.CreateOpenMap(80, 48);
        var player = new Player();
        player.X = 10;
        player.Y = 10;
        map.SetActorPosition(player, player.X, player.Y);

        var goblin = Goblin.Create(1, new DotNetRandom(42));
        goblin.X = 15;
        goblin.Y = 10;
        map.AddMonster(goblin);

        var state = new GameState(map, player);
        var commands = CreateCommands(state);
        commands.ActivateMonsters();

        commands.EndPlayerTurn();

        Assert.Equal(14, goblin.X);
    }
}
