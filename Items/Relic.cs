using csharp_roguelike_rpg.Systems;
namespace csharp_roguelike_rpg.Items;

public class Relic : Item
{
    public Modifier Effect { get; private set; }

    public Relic(string name, string description, Modifier effect) : base(name, description)
    {
        Effect = effect;
    }
}