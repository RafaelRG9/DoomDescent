public class Player : Character
{
    public int Level { get; set; }
    public int Experience { get; set; }
    public List<Item> Inventory { get; private set; }

    public Player(string name, int health, int strength, int dexterity)
        : base(name, health, strength, dexterity)
    {
        Level = 1;
        Experience = 0;
        Inventory = new List<Item>();
    }
}