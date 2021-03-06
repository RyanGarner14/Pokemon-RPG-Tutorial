using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PokemonBase _base;
    int level;

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _base = pBase;
        level = pLevel;
    }

    public int MaxHP
    {
        get { return getStatFromLevel(_base.MaxHP) + 5; }
    }
    public int Attack
    {
        get { return getStatFromLevel(_base.Attack); }
    }
    public int Defense
    {
        get { return getStatFromLevel(_base.Defense); }
    }
    public int SpAttack
    {
        get { return getStatFromLevel(_base.SpAttack); }
    }
    public int SpDefense
    {
        get { return getStatFromLevel(_base.SpDefense); }
    }
    public int Speed
    {
        get { return getStatFromLevel(_base.Speed); }
    }

    private int getStatFromLevel(int stat)
    {
        return (Mathf.FloorToInt((stat * level) / 100f) + 5);
    }
}
