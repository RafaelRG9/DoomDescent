using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class BossMinotaurGuardian : Monster
{
    public BossMinotaurGuardian(Item? bossLoot)
        : base("Minotaur Guardian", 180, 22, 12, 1200, 5, bossLoot)
    {

    }
}