namespace csharp_roguelike_rpg.Characters;

public abstract class Character
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int Energy { get; set; }
    public int MaxEnergy { get; set; }
    public Dictionary<string, int> Stats { get; private set; }
    public bool IsDefending { get; set; }

    public Character(string name, int health, int strength, int dexterity, int intellect)
    {
        Name = name;
        MaxHealth = health;
        Health = health;
        Stats = new Dictionary<string, int>
        {
            {"Strength", strength},
            {"Dexterity", dexterity},
            {"Intellect", intellect}
        };
    }
}