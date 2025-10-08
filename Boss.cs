public class Boss : Monster
{
    public Boss(Item? bossLoot)
        : base("Dragon", 100, 15, 8, 1000, 10, bossLoot)
    {
        
    }
}