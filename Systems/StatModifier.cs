namespace csharp_roguelike_rpg.Systems;

using System.Net;
using csharp_roguelike_rpg.Characters;

public class StatModifier : Modifier
{
    private readonly string _statToModify;
    private readonly int _amount;
    private readonly string _statToPenalize;
    private readonly int _penalty;

    public StatModifier(string name, string description, Rarity rarity, string statToModify, int amount, string statToPenalize, int penalty)
    {
        Name = name;
        Description = description;
        Rarity = rarity;
        _statToModify = statToModify;
        _amount = amount;
        _statToPenalize = statToPenalize;
        _penalty = penalty;
    }

    public override void Apply(Player player)
    {
        if (player.Stats.ContainsKey(_statToPenalize))
        {
            if (_statToPenalize == "null")
            {
                player.Stats[_statToModify] += _amount;
                UIManager.SlowPrint($"Your {_statToModify} has permanently increased by {_amount}!", ConsoleColor.Green);
            }
            else if (_statToPenalize != "null")
            {
                player.Stats[_statToModify] += _amount;
                player.Stats[_statToPenalize] -= _penalty;
                UIManager.SlowPrint($"Your {_statToModify} has permanently increased by {_amount}!", ConsoleColor.Green);
                UIManager.SlowPrint($"Your {_statToPenalize} has permanently decreased by {_amount}!", ConsoleColor.Red);
            }

        }
        else
        {
            player.Stats[_statToModify] += _amount;
            UIManager.SlowPrint($"Your {_statToModify} has permanently increased by {_amount}!", ConsoleColor.Green);
        }
    }
}