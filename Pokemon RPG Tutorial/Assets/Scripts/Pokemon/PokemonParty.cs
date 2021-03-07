using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemon;

    public List<Pokemon> Pokemon
    {
        get { return pokemon; }
    }

    void Start()
    {
        foreach(var p in pokemon)
        {
            p.Init();
        }
    }

    public Pokemon getHealthyPokemon()
    {
        return pokemon.Where(x => x.HP > 0).FirstOrDefault(); // Returns first pokemon in party which is not fainted
    }

}
