public class Monster : Character
{
    public Item? Loot { get; set; }

    public Monster(string name, int health, int strength, int dexterity, Item? loot = null)
        : base(name, health, strength, dexterity)
    {
            Loot = loot;
    }
}