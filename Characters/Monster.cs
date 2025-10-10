using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class Monster : Character
{
    public Item? Loot { get; set; }
    public int ExperienceValue { get; set; }

    public Monster(string name, int health, int strength, int dexterity, int experience, int intellect, Item? loot = null)
        : base(name, health, strength, dexterity, intellect)
    {
        ExperienceValue = experience;
        Loot = loot;
    }
}