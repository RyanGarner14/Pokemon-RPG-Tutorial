using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been Poisoned.",
                OnAfterTurn =  (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} was hurt due to Poison.");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been Burned.",
                OnAfterTurn =  (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} was hurt due to it's Burn.");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralysis",
                StartMessage = "has been Paralyzed.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if ( Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is paralyzed and can't move.");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has been put to Sleep.",
                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} has Woken up.");
                        return true;
                    }
                    pokemon.StatusTime--;

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fast Asleep.");

                    return false;
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "has been Frozen.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if ( Random.Range(1,5) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} has been Thawed out.");
                    }
                    return false;
                }
            }
        },
        {
            ConditionID.conf,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "has been Confused.",
                OnStart = (Pokemon pokemon) =>
                {
                    // Confused for 1-3 turns
                    pokemon.VolatileStatusTime = Random.Range(1,4);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} has snapped out of Confusion.");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;

                    // 50% chance to complete move
                    if(Random.Range(1,3) == 1)
                        return true;

                    // Hurt by Confusion                    
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is Confused.");
                    pokemon.UpdateHP(pokemon.MaxHP / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself in Confusion.");
                    return false;
                }
            }
        },
    };
}

public enum ConditionID
{
    none, psn, brn, slp, par, frz, conf
}
