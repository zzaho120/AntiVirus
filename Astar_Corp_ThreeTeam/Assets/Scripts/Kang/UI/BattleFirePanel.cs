using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleFirePanel : MonoBehaviour
{
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI critRateText;

    public void SetDmgText(PlayerableChar player)
    {
        var weapon = player.characterStats.weapon.curWeapon;

        dmgText.text = $"{weapon.minDamage}~{weapon.maxDamage}";
        critRateText.text = $"{weapon.critRate}%";
    }
}
