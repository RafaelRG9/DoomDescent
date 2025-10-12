using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class Ogre : Monster
{
    public Ogre(Item? loot)
        : base("Ogre", 60, 14, 1, 2, 15, loot)
    {

    }
}