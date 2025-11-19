using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class WildMagic : Ability
{
    public WildMagic()
    {
        Name = "Wild Magic";
        Description = "Unleash raw chaos. Could heal, hurt, or backfire.";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        Random rng = new Random();
        int roll = rng.Next(1, 4); // 1 to 3

        switch (roll)
        {
            case 1: // Fire
                int fireDmg = caster.Stats["Intellect"] * 3;
                target.Health -= fireDmg;
                UIManager.SlowPrint("Wild Magic surges as FIRE! Dealing massive damage.", ConsoleColor.Red);
                break;
            case 2: // Heal
                caster.Health += 20;
                if(caster.Health > caster.MaxHealth) caster.Health = caster.MaxHealth;
                UIManager.SlowPrint("Wild Magic surges as LIGHT! You are healed by 20.", ConsoleColor.Green);
                break;
            case 3: // Backfire
                caster.Health -= 5;
                UIManager.SlowPrint("Wild Magic BACKFIRES! You singe your eyebrows and loose 5 health.", ConsoleColor.DarkGray);
                break;
        }
    }
}