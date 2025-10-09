using csharp_roguelike_rpg.Characters;

public abstract class Modifier
{
    public string? Name { get; protected set; }
    public string? Description { get; protected set; }
    public Rarity Rarity { get; protected set; }

    public abstract void Apply(Player player);
}