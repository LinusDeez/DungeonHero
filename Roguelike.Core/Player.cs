namespace Roguelike.Core;

public class Player : Actor
{
    public Player()
    {
        Name = "Rogue";
        Awareness = 15;
        Attack = 2;
        AttackChance = 50;
        Defense = 2;
        DefenseChance = 40;
        Gold = 0;
        Health = 100;
        MaxHealth = 100;
        Speed = 10;
    }
}