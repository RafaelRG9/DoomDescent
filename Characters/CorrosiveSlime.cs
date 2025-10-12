using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class CorrosiveSlime : Monster
{
    public CorrosiveSlime(Item? loot)
        : base("Corrosive Slime", 30, 7, 2, 1, 35, loot)
    {

    }
}