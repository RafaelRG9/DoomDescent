using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.Characters;

public class Monster : Character
{
    public int Level { get; private set; }
    public Item? Loot { get; set; }
    public int ExperienceValue { get; set; }

    public Monster(string name, int health, int strength, int dexterity, int experience, int intellect, Item? loot = null)
        : base(name, health, strength, dexterity, intellect)
    {
        Level = 1;
        ExperienceValue = experience;
        Loot = loot;
    }

    public Monster(Monster other)
        : base(other.Name, other.MaxHealth, other.Stats["Strength"], other.Stats["Dexterity"], other.Stats["Intellect"])
    {
        ExperienceValue = other.ExperienceValue;
        Loot = other.Loot;
        Level = other.Level;
    }

    public Monster Clone()
    {
        return new Monster(this);
    }

    public void ScaleStats(int level)
    {
        Level = level;

        // Get the original base stats from the constructor
        int baseHealth = MaxHealth;
        int baseStrength = Stats["Strength"];
        int baseDexterity = Stats["Dexterity"];
        int baseIntellect = Stats["Intellect"];

        // Apply the scaling formula
        MaxHealth = baseHealth + (level * 5);
        Health = MaxHealth;
        Stats["Strength"] = baseStrength + (level * 2);
        Stats["Dexterity"] = baseDexterity + (level * 1);
        Stats["Intellect"] = baseIntellect + (level * 1);
    }
}