using DungeonHero.Core;
using DungeonHero.Core.Monsters;
using SadRogue.Primitives;

namespace DungeonHero.Client;

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
