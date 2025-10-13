using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class BossLichLord : Monster
{
    public BossLichLord(Item? bossLoot)
        : base("Lich Lord Azmoth", 100, 8, 10, 2500, 20, bossLoot)
    {

    }
}