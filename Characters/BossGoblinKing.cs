using csharp_roguelike_rpg.Items;
namespace csharp_roguelike_rpg.Characters;

public class BossGoblinKing : Monster
{
    public BossGoblinKing(Item? bossLoot)
        : base("Goblin King Gnash", 80, 14, 8, 500, 6, bossLoot)
    {

    }
}