namespace csharp_roguelike_rpg.Items;
public class Armor : Item
{
    public int Defense { get; set; }

    public Armor(string name, string description, int defense) : base(name, description)
    {
        Defense = defense;
    }
}