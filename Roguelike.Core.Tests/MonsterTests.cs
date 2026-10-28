using Roguelike.Core.Monsters;
using RogueSharp.Random;

namespace Roguelike.Core.Tests;

public class MonsterTests
{
    [Fact]
    public void Goblin_Create_HasHealthBetween2And10()
    {
        var goblin = Goblin.Create(1, new DotNetRandom(42));

        Assert.InRange(goblin.Health, 2, 10);
        Assert.Equal(goblin.MaxHealth, goblin.Health);
    }

    [Fact]
    public void PlaceMonsters_MonstersBlockTheirCell()
    {
        var generator = new MapGenerator(80, 48, 20, 13, 7, new DotNetRandom(42));
        var map = generator.CreateMap();

        generator.PlaceMonsters(map);

        Assert.NotEmpty(map.Monsters);
        Assert.All(map.Monsters, monster => Assert.False(map.IsWalkable(monster.X, monster.Y)));
    }
}
