using csharp_roguelike_rpg.Characters;
using csharp_roguelike_rpg.Systems;

namespace csharp_roguelike_rpg.Abilities;

public class CoinToss : Ability
{
    public CoinToss()
    {
        Name = "Coin Toss";
        Description = "Flip a coin. Heads: Deal 500% Dexterity damage. Tails: Deal 1 damage.";
        EnergyCost = 5;
    }

    public override void Use(Character caster, Character target)
    {
        Random rng = new Random();
        int flip = rng.Next(2); // 0 or 1

        if (flip == 0) // Heads
        {
            int damage = caster.Stats["Dexterity"] * 5;
            target.Health -= damage;
            UIManager.SlowPrint($"{caster.Name} flips a coin... HEADS! A massive strike dealing {damage} damage!", ConsoleColor.Yellow);
        }
        else // Tails
        {
            target.Health -= 1;
            UIManager.SlowPrint($"{caster.Name} flips a coin... TAILS. An awkward stumble dealing 1 damage.", ConsoleColor.Gray);
        }
    }
}