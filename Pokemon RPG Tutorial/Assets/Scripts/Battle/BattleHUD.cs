using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text statusText;
    [SerializeField] HPBar hpBar;

    [SerializeField] Color psnColour;
    [SerializeField] Color brnColour;
    [SerializeField] Color slpColour;
    [SerializeField] Color parColour;
    [SerializeField] Color frzColour;

    Pokemon _pokemon;
    Dictionary<ConditionID, Color> statusColours;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl " + pokemon.Level;

        hpBar.setHP((float)pokemon.HP / pokemon.MaxHP);

        statusColours = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColour },
            {ConditionID.brn, brnColour },
            {ConditionID.slp, slpColour },
            {ConditionID.par, parColour },
            {ConditionID.frz, frzColour }
        };

        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }

    void SetStatusText()
    {
        if (_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColours[_pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if(_pokemon.HpChanged)
            yield return hpBar.setHPSmooth((float)_pokemon.HP / _pokemon.MaxHP);
        _pokemon.HpChanged = false;
    }
}
