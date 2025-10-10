namespace csharp_roguelike_rpg.Systems;
using csharp_roguelike_rpg.Characters;

public class StatModifier : Modifier
{
    private readonly string _statToModify;
    private readonly int _amount;

    public StatModifier(string name, string description, Rarity rarity, string statToModify, int amount)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        _statToModify = statToModify;
        _amount = amount;
    }

    public override void Apply(Player player)
    {
        if (player.Stats.ContainsKey(_statToModify))
        {
            player.Stats[_statToModify] += _amount;
            UIManager.SlowPrint($"Your {_statToModify} has permanently increased by {_amount}!", ConsoleColor.Green);
        }   
    }
}