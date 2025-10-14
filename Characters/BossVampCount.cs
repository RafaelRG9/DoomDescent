using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class BossVampCount : Monster
{
    public BossVampCount(Item? bossLoot)
        : base("Vampire Count Valerius", 200, 18, 18, 7500, 15, bossLoot)
    {

    }
}