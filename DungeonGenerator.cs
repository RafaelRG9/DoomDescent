public class DungeonGenerator
{
    // We create one Random object and reuse it for better randomness.
    private Random _random = new Random();

    public Room Generate(int numberOfRooms)
    {
        // Dictionary to hold the entire map
        var map = new Dictionary<string, Room>();

        // Create starting room at coordinates (0,0)
        Room currentRoom = new Room(0, 0);
        map.Add("0,0", currentRoom);

        // Save first room as a reference to come back to it at the end
        Room startRoom = currentRoom;

        // --- MAIN LOOP ---
        // Loop for 'numberOfRooms - 1' since room one is already created
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
            Item goblinHide = new Item("Goblin Hide", "Yuck! What did you do to loot this? leathery and tough but, extremely smelly!");

            // Check if a room already exists in the new coordinate.
            // TryGetValue checks the dictionary and if the key exists,
            // it will put the room in the nextRoom variable and return true
            if (!map.TryGetValue(nextPositionKey, out nextRoom))
            {
                // Create a new room if it doesn't exists
                nextRoom = new Room(nextX, nextY);
                map.Add(nextPositionKey, nextRoom);

                // --- MONSTER & ITEM SPAWNING ---
                if (_random.Next(100) < 50)
                {
                    // define loot, create monster, and add to room's list
                    Monster goblin = new Monster("Goblin", 20, 8, 3, 50, goblinHide);
                    nextRoom.MonstersInRoom.Add(goblin);

                    // Update the room's description to mention the monster
                    nextRoom.Name = "Monster Room";
                    nextRoom.Description = "A foul smell hangs in the air. A Goblin glares at you!";
                }
                else
                {
                    nextRoom.ItemsInRoom.Add(goblinHide);

                    nextRoom.Name = "Treasure Room";
                    nextRoom.Description = "Fortune favors you!";
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

            // Update walker's position to the new room, wether it is a new or existing room
            currentRoom = nextRoom;
        }
        // --- RETURN TO START ---
        return startRoom;
    }
}