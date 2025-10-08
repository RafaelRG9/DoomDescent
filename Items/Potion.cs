public class Potion : Item
{
    public int HealthToRestore { get; set; }

    public Potion(string name, string description, int healthToRestore) : base(name, description)
    {
        HealthToRestore = healthToRestore;
    }
}