using Roguelike.Core;
using Roguelike.Core.Monsters;
using SadRogue.Primitives;

namespace Roguelike.Client;

public static class MonsterLook
{
    public static char Glyph(Monster monster) => monster switch
    {
        Goblin => 'g',
        _ => '?',
    };

    public static Color Color(Monster monster) => monster switch
    {
        Goblin => Colors.Goblin,
        _ => Colors.Text,
    };
}
