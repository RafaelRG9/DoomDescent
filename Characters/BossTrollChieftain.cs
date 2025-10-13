using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class BossTrollChieftain : Monster
{
    public BossTrollChieftain(Item? bossLoot)
        : base("Troll Chieftain Grokk", 150, 18, 2, 1200, 3, bossLoot)
    {

    }
}