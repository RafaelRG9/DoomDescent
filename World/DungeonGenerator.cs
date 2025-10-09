namespace csharp_roguelike_rpg.Characters;

public class DungeonGenerator
{
    private Random _random = new Random();
    private GameData _gameData;

    public DungeonGenerator(GameData gameData)
    {
        _gameData = gameData;
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
                if (i == numberOfRooms - 2)
                {
                    // ---- SPAWN THE BOSS ---
                    int randomIndex = _random.Next(_gameData.MasterBossTable.Count);
                    Monster randomBoss = _gameData.MasterBossTable[randomIndex];
                    Monster BossForRoom = new Monster(randomBoss.Name, randomBoss.MaxHealth, randomBoss.Stats["Strength"], randomBoss.Stats["Dexterity"], randomBoss.ExperienceValue, randomBoss.Stats["Intellect"], randomBoss.Loot);
                    nextRoom.MonstersInRoom.Add(BossForRoom);
                }
                else if (_random.Next(100) < 50)
                {
                    // define loot, create monster, and add to room's list
                    int randomIndex = _random.Next(_gameData.MasterMonsterTable.Count);
                    Monster randomMonster = _gameData.MasterMonsterTable[randomIndex];
                    Monster monsterForRoom = new Monster(randomMonster.Name, randomMonster.MaxHealth, randomMonster.Stats["Strength"], randomMonster.Stats["Dexterity"], randomMonster.ExperienceValue, randomMonster.Stats["Intellect"], randomMonster.Loot);
                    nextRoom.MonstersInRoom.Add(monsterForRoom);

                    // Update the room's description to mention the monster
                    nextRoom.Name = "Monster Room";
                    nextRoom.Description = $"The air is thick with the stench of death and decay.";
                }
                else
                {
                    int randomIndex = _random.Next(_gameData.MasterLootTable.Count);
                    Item lootItem = _gameData.MasterLootTable[randomIndex];
                    nextRoom.ItemsInRoom.Add(lootItem);

                    nextRoom.Name = "Treasure Room";
                    nextRoom.Description = $"You feel a sense of calm here, a brief respite from the darkness.";
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
            // Main walker logic: 80% chance to continue the current path. 
            // The 20% chance to jump to a random existing room encourages branching for a less linear map.
            if (_random.Next(100) < 80)
            {
                currentRoom = nextRoom;
            }
            else
            {
                var allRooms = map.Values.ToList();
                currentRoom = allRooms[_random.Next(allRooms.Count)];
            }
        }
        // --- RETURN TO START ---
        return startRoom;
    }
}