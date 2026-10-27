namespace Roguelike.Core;

public class Player : Actor
{
    public Player()
    {
        Name = "Rogue";
        Awareness = 15;
        X = 10;
        Y = 10;
    }
}