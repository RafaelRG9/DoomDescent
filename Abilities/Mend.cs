using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class Mend : Ability
{
    public Mend()
    {
        Name = "Mend";
        Description = "You heal yourself, as self explanatory as self explanatory gets!. Heals for 300% of intellect";
        EnergyCost = 8;
    }

    public override void Use(Character caster, Character target)
    {
        // Scaling based on Intellect
        int healAmount = caster.Stats["Intellect"] * 3;

        caster.Health += healAmount;

        // Prevent overhealing
        if (caster.Health > caster.MaxHealth)
        {
            caster.Health = caster.MaxHealth;
        }

        UIManager.SlowPrint($"{caster.Name} casts Mend and recovers {healAmount} health!", ConsoleColor.Green);
    }
}