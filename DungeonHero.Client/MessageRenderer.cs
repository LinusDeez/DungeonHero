using DungeonHero.Core;
using SadConsole;

namespace DungeonHero.Client;

public static class MessageRenderer
{
    public static void Draw(MessageLog log, SadConsole.Console console)
    {
        console.Clear();
        console.Fill(background: Swatch.DbDeepWater);

        int row = 1;
        foreach (string line in log.Lines)
        {
            console.Print(1, row, line, Colors.Text);
            row++;
        }
    }
}
