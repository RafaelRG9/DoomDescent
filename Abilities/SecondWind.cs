using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class SecondWind : Ability
{
    public SecondWind()
    {
        Name = "Second Wind";
        Description = "Adrenaline surges. Fully restore Health and Energy.";
        EnergyCost = 0;
    }

    public override void Use(Character caster, Character target)
    {
        caster.Health = caster.MaxHealth;
        caster.Energy = caster.MaxEnergy;

        UIManager.SlowPrint($"{caster.Name} roars with renewed vigor! Fully healed!", ConsoleColor.Green);
    }
}