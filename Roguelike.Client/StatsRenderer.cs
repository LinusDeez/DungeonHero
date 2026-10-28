using Roguelike.Core;
using SadConsole;

namespace Roguelike.Client;

public static class StatsRenderer
{
    public static void Draw(GameState state, SadConsole.Console console)
    {
        Player player = state.Player;

        console.Clear();
        console.Fill(background: Swatch.DbOldStone);

        console.Print(1, 1, $"Name:     {player.Name}", Colors.Text);
        console.Print(1, 3, $"Health:   {player.Health}/{player.MaxHealth}", Colors.Text);
        console.Print(1, 5, $"Attack:   {player.Attack} ({player.AttackChance}%)", Colors.Text);
        console.Print(1, 7, $"Defense:  {player.Defense} ({player.DefenseChance}%)", Colors.Text);
        console.Print(1, 9, $"Gold:     {player.Gold}", Colors.Gold);

        int i = 0;
        foreach (Monster monster in state.Map.GetMonstersInFov())
        {
            DrawMonsterStats(monster, 13 + i * 3, console);
            i++;
        }
    }

    private static void DrawMonsterStats(Monster monster, int y, SadConsole.Console console)
    {
        // Zeile 1: Symbol und Name
        console.Print(1, y, MonsterLook.Glyph(monster).ToString(), MonsterLook.Color(monster));
        console.Print(2, y, $": {monster.Name}", Colors.Text);

        // Zeile 2: Balken
        const int barX = 3;
        const int barWidth = 10;
        int barY = y + 1;

        int width = monster.Health * barWidth / monster.MaxHealth;
        int remainingWidth = barWidth - width;

        console.Fill(barX, barY, width, background: Colors.HealthBar);
        console.Fill(barX + width, barY, remainingWidth, background: Colors.HealthBarEmpty);

        // Text in die Mitte des Balkens
        string text = $"{monster.Health}/{monster.MaxHealth}";
        int textX = barX + (barWidth - text.Length) / 2;
        console.Print(textX, barY, text, Colors.Text);
    }
}
