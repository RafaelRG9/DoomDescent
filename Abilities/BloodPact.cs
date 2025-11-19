using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class BloodPact : Ability
{
    public BloodPact()
    {
        Name = "Blood Pact";
        Description = "Sacrifice 10 Health to restore 15 Energy.";
        EnergyCost = 0;
    }

    public override void Use(Character caster, Character target)
    {
        if (caster.Health <= 10)
        {
            UIManager.SlowPrint("You are too weak to pay the blood price!", ConsoleColor.Red);
            return;
        }

        caster.Health -= 10;
        caster.Energy += 15;

        // Over energize prevention
        if (caster.Energy > caster.MaxEnergy) caster.Energy = caster.MaxEnergy;

        UIManager.SlowPrint($"{caster.Name} cuts their hand, fueling magic with blood.", ConsoleColor.DarkRed);
        UIManager.SlowPrint($"-10 HP, +15 Energy.", ConsoleColor.Red);
    }
}