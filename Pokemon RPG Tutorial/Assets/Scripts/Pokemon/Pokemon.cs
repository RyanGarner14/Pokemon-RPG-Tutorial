using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base
    {
        get { return _base; }
    }
    public int Level
    {
        get { return level; }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public void Init()
    {
        HP = MaxHP;

        // Generate Moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
    }

    public int MaxHP
    {
        get { return getStatFromLevel(Base.MaxHP) + 5; }
    }
    public int Attack
    {
        get { return getStatFromLevel(Base.Attack); }
    }
    public int Defense
    {
        get { return getStatFromLevel(Base.Defense); }
    }
    public int SpAttack
    {
        get { return getStatFromLevel(Base.SpAttack); }
    }
    public int SpDefense
    {
        get { return getStatFromLevel(Base.SpDefense); }
    }
    public int Speed
    {
        get { return getStatFromLevel(Base.Speed); }
    }

    private int getStatFromLevel(int stat)
    {
        return (Mathf.FloorToInt((stat * Level) / 100f) + 5);
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25)
            critical = 2f;

        float type = TypeChart.getEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.getEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };

        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.IsSpecial) ? SpDefense : Defense;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move getRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }

    public float Critical { get; set; }

    public float TypeEffectiveness { get; set; }
}