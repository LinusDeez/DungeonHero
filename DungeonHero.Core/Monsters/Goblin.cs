using RogueSharp.DiceNotation;
using RogueSharp.Random;

namespace DungeonHero.Core.Monsters;

public class Goblin : Monster
{
    public static Goblin Create(int level, IRandom random)
    {
        int health = Dice.Roll("2D5", random);

        return new Goblin
        {
            Name = "Goblin",
            Health = health,
            MaxHealth = health,
            Attack = Dice.Roll("1D3", random) + level / 3,
            AttackChance = Dice.Roll("25D3", random),
            Defense = Dice.Roll("1D3", random) + level / 3,
            DefenseChance = Dice.Roll("10D4", random),
            Gold = Dice.Roll("5D5", random),
            Awareness = 10,
            Speed = 14,
        };
    }
}
