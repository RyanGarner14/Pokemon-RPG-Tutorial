﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color highlightColour;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;

        hpBar.setHP((float)pokemon.HP / pokemon.MaxHP);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = highlightColour;
        else
            nameText.color = Color.black;        
    }
}
