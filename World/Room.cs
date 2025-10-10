using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Items;

namespace csharp_roguelike_rpg.World;
public class Room
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Monster> MonstersInRoom { get; set; }
    public List<Item> ItemsInRoom { get; set; }

    // Coordinates on the map grid
    public int X { get; set; }
    public int Y { get; set; }

    // Connection to other rooms
    public Room? North { get; set; }
    public Room? South { get; set; }
    public Room? East { get; set; }
    public Room? West { get; set; }

    public Room(int x, int y)
    {
        X = x;
        Y = y;
        Name = "Empty Room";
        Description = "A dusty, featureless stone room.";
        MonstersInRoom = new List<Monster>();
        ItemsInRoom = new List<Item>();
    }
}