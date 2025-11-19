using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class Vengeance : Ability
{
    public Vengeance()
    {
        Name = "Vengeance";
        Description = "Convert your pain into power. Deals Strength damage + the amount of Health you are missing.";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        int missingHealth = caster.MaxHealth - caster.Health;
        int baseDamage = caster.GetTotalStrength();
        
        int totalDamage = baseDamage + missingHealth;

        target.Health -= totalDamage;

        UIManager.SlowPrint($"{caster.Name} lashes out in pain!", ConsoleColor.Red);
        UIManager.SlowPrint($"Deals {totalDamage} damage ({missingHealth} from missing health!)", ConsoleColor.DarkRed);
    }
}