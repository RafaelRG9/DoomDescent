using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;
public class Goblin : Monster
{
    public Goblin(Item? loot)
        : base("Goblin", 20, 8, 3, 3, 50, loot)
    {

    }
}