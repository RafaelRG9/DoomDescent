using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class Boss : Monster
{
    public Boss(Item? bossLoot)
        : base("Ignis, the Ancient Flame", 500, 30, 10, 20000, 20, bossLoot)
    {

    }
}