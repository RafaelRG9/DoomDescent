using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;
public class Orc : Monster
{
    public Orc(Item? orcLoot)
        : base("Orc", 40, 12, 2, 100, 2, orcLoot)
    {

    }
}