using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
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

    public bool TakeDamage(Move move, Pokemon attacker)
    {
        float type = TypeChart.getEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.getEffectiveness(move.Base.Type, this.Base.Type2);

        float modifiers = Random.Range(0.85f, 1f) * type;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if(HP <= 0)
        {
            HP = 0;
            return true;
        }
        return false;
    }

    public Move getRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
