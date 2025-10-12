using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class GiantSpider : Monster
{
    public GiantSpider(Item? loot)
        : base("Giant Spider", 25, 9, 15, 2, 80, loot)
    {

    }
}