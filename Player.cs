public class Player : Character
{
    public int level { get; set; }
    public int Experience { get; set; }

    public Player(string name, int health, int strength, int dexterity)
        : base(name, health, strength, desterity)
    {
        level = 1;
        Experience = 0;
    }
}