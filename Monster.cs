public class Monster : Character
{
    public Item? Loot { get; set; }
    public int ExperienceValue { get; set; }

    public Monster(string name, int health, int strength, int dexterity, int experience, Item? loot = null)
        : base(name, health, strength, dexterity)
    {
        ExperienceValue = experience;
        Loot = loot;
    }
}