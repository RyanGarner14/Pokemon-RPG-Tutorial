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

    public Dictionary<Stat, int> Stats { get; private set; }

    public Dictionary<Stat, int> StatBoosts { get; private set; }

    public void Init()
    {
        // Generate Moves
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }

        CalculateStats();

        HP = MaxHP;

        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0 },
            {Stat.Defense, 0 },
            {Stat.SpAttack, 0 },
            {Stat.SpDefense, 0 },
            {Stat.Speed, 0 },
        };
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, getStatFromLevel(Base.Attack));        
        Stats.Add(Stat.Defense, getStatFromLevel(Base.Defense));
        Stats.Add(Stat.SpAttack, getStatFromLevel(Base.SpAttack));
        Stats.Add(Stat.SpDefense, getStatFromLevel(Base.SpDefense));
        Stats.Add(Stat.Speed, getStatFromLevel(Base.Speed));

        MaxHP = Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10;
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        // Apply stat boosts
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        else
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);

        return statVal;
    }

    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach(var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }
    
    public int Attack   { get { return GetStat(Stat.Attack);   } }
    public int Defense  { get { return GetStat(Stat.Defense);  } }
    public int SpAttack { get { return GetStat(Stat.SpAttack); } }
    public int SpDefense{ get { return GetStat(Stat.SpDefense);} }
    public int Speed    { get { return GetStat(Stat.Speed);    } }
    public int MaxHP    { get; private set; }

    private int getStatFromLevel(int stat)
    {
        return (Mathf.FloorToInt((stat * Level) / 100f) + 5);
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        // Set Critical Hit rate
        float critical = 1f;
        if (Random.value * 100f <= 6.25)
            critical = 2f;

        // Check Type Effectiveness
        float type = TypeChart.getEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.getEffectiveness(move.Base.Type, this.Base.Type2);

        // Update Damage Details
        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };

        // Switch between using Special/Physical stats based on move used
        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAttack : attacker.Attack;
        float defense = (move.Base.Category == MoveCategory.Special) ? SpDefense : Defense;

        // Apply pokemon damage calculations
        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        // Reduce HP by calc'd damage
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