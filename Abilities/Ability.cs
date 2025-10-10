using csharp_roguelike_rpg.Characters;
namespace csharp_roguelike_rpg.Abilities;

public abstract class Ability
{
    public string? Name { get; protected set; }
    public string? Description { get; protected set; }
    public int EnergyCost { get; protected set; }

    public abstract void Use(Character caster, Character target);
}