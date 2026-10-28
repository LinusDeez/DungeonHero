using Roguelike.Core;
using SadConsole;

namespace Roguelike.Client;

public static class StatsRenderer
{
    public static void Draw(Player player, SadConsole.Console console)
    {
        console.Clear();
        console.Fill(background: Swatch.DbOldStone);

        console.Print(1, 1, $"Name:     {player.Name}", Colors.Text);
        console.Print(1, 3, $"Health:   {player.Health}/{player.MaxHealth}", Colors.Text);
        console.Print(1, 5, $"Attack:   {player.Attack} ({player.AttackChance}%)", Colors.Text);
        console.Print(1, 7, $"Defense:  {player.Defense} ({player.DefenseChance}%)", Colors.Text);
        console.Print(1, 9, $"Gold:     {player.Gold}", Colors.Gold);
    }
}
