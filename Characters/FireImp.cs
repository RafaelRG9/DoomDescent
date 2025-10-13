using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class FireImp : Monster
{
    public FireImp(Item? loot)
        : base("Fire Imp", 12, 3, 14, 70, 8, loot)
    {

    }
}