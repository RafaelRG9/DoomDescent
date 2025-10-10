namespace csharp_roguelike_rpg.Items;
public class Weapon : Item
{
    public int Damage { get; set; }

    public Weapon(string name, string description, int damage) : base(name, description)
    {
        Damage = damage;
    }
}