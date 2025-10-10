using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class Boss : Monster
{
    public Boss(Item? bossLoot)
        : base("Dragon", 100, 15, 8, 1000, 10, bossLoot)
    {

    }
}