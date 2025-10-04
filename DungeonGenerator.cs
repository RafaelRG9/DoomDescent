public class DungeonGenerator
{
    // We create one Random object and reuse it for better randomness.
    private Random _random = new Random();

    // List to hold all possible items that can spawn in rooms
    private List<Item> _masterLootTable;

    public DungeonGenerator()
    {
        _masterLootTable = new List<Item>();

        // Populate the master loot table with some items
        _masterLootTable.Add(new Item("Goblin Hide", "Yuck! What did you do to loot this? leathery and tough but, extremely smelly!"));
        _masterLootTable.Add(new Armor("Rusty Armor", "Well, better than nothing!", 2));
        _masterLootTable.Add(new Weapon("Rusty Sword", "An old sword, as sharp as rolling pin!. At least it could give Tetanus", 2));
        _masterLootTable.Add(new Item("Small Potion", "Wouldn't hurt to drink this, wouldn't help either"));
    }

    public Room Generate(int numberOfRooms)
    {
        // --- INITIALIZATION ---
        var map = new Dictionary<string, Room>();
        Room currentRoom = new Room(0, 0);
        map.Add("0,0", currentRoom);
        Room startRoom = currentRoom;

        // --- MAIN LOOP ---
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            // Choose a random direction (0=North, 1=East, 2=South, 3=West)
            int direction = _random.Next(4);

            // Calculate coordinates of potential next room
            int nextX = currentRoom.X;
            int nextY = currentRoom.Y;
            switch (direction)
            {
                case 0: nextY++; break; //  North
                case 1: nextX++; break; //  East
                case 2: nextY--; break; //  South
                case 3: nextX--; break; //  West
            }

            string nextPositionKey = $"{nextX},{nextY}";
            Room? nextRoom;

            // Check if a room already exists at the new coordinates
            if (!map.TryGetValue(nextPositionKey, out nextRoom))
            {
                // Create a new room if it doesn't exists
                nextRoom = new Room(nextX, nextY);
                map.Add(nextPositionKey, nextRoom);

                // --- MONSTER & ITEM SPAWNING ---
                if (_random.Next(100) < 50)
                {
                    // define loot, create monster, and add to room's list
                    Item? goblinLoot = _masterLootTable.Find(item => item.Name == "Goblin Hide");
                    Monster goblin = new Monster("Goblin", 20, 8, 3, 50, goblinLoot);
                    nextRoom.MonstersInRoom.Add(goblin);

                    // Update the room's description to mention the monster
                    nextRoom.Name = "Monster Room";
                    nextRoom.Description = "A foul smell hangs in the air. A Goblin glares at you!";
                }
                else
                {
                    int randomIndex = _random.Next(_masterLootTable.Count);
                    Item lootItem = _masterLootTable[randomIndex];
                    nextRoom.ItemsInRoom.Add(lootItem);

                    nextRoom.Name = "Treasure Room";
                    nextRoom.Description = $"Fortune favors you! There's a {lootItem.Name} here. GO GRAB IT!";
                }

                // Link the new room to the current room
                switch (direction)
                {
                    case 0: // Moved North
                        currentRoom.North = nextRoom;
                        nextRoom.South = currentRoom;
                        break;
                    case 1: // Moved East
                        currentRoom.East = nextRoom;
                        nextRoom.West = currentRoom;
                        break;
                    case 2: // Moved South
                        currentRoom.South = nextRoom;
                        nextRoom.North = currentRoom;
                        break;
                    case 3: // Moved West
                        currentRoom.West = nextRoom;
                        nextRoom.East = currentRoom;
                        break;
                }
            }
            currentRoom = nextRoom;
        }
        // --- RETURN TO START ---
        return startRoom;
    }
}